using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ProxySharp.Tcp.ConnectionProviders.Abstractions;
using ProxySharp.Tcp.ConnectionProviders.Socks4.Exceptions;
using ProxySharp.Tcp.Extensions;

namespace ProxySharp.Tcp.ConnectionProviders.Socks4
{
    /// <summary>
    /// Connection provider through socks4 proxy
    /// </summary>
    public class Socks4ConnectionProvider : IConnectionProvider
    {
        /// <summary>
        /// IP or hostname used to connect to proxy
        /// </summary>
        public string ProxyHost { get; }

        /// <summary>
        /// Port used to connect to proxy
        /// </summary>
        public int ProxyPort { get; }


        /// <summary>
        /// Used to create connection provider through defined proxy
        /// </summary>
        /// <param name="proxyHost">Proxy host</param>
        /// <param name="proxyPort">Proxy port</param>
        /// <exception cref="ArgumentOutOfRangeException">Port is not between 0 and 65535</exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Socks4ConnectionProvider(string proxyHost, int proxyPort)
        {
            if (proxyPort > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(proxyPort));

            if (proxyPort < 0)
                throw new ArgumentOutOfRangeException(nameof(proxyPort));


            ProxyHost = proxyHost ?? throw new ArgumentNullException(nameof(proxyHost));
            ProxyPort = proxyPort;
        }

        /// <summary>
        /// Create a connection through proxy
        /// </summary>
        /// <remarks>Uses first ip of hostname</remarks>
        /// <param name="host">Destination host</param>
        /// <param name="port">Destination port</param>
        /// <param name="cancellationToken">Cancellation token. Should not be `default` because connecting can take infinite time</param>
        /// <returns>Connected TcpClient</returns>
        /// <exception cref="ArgumentOutOfRangeException">Port is not between 0 and 65535</exception>
        /// <exception cref="ArgumentException">Host is null or have no addresses in `Dns.GetHostAddresses`</exception>
        /// <exception cref="SocksUnknownException">Happened unknown error</exception>
        /// <exception cref="SocksResponseException">Socks4 proxy returned error code</exception>
        /// <exception cref="SocksConnectingException">Cannot connect to proxy</exception>
        public async Task<TcpClient> Connect(string host, int port, CancellationToken cancellationToken = default)
        {
            if (port > ushort.MaxValue)
                throw new ArgumentOutOfRangeException(nameof(port), $"{nameof(port)} must be between 0 and 65535");

            if (port < 0)
                throw new ArgumentOutOfRangeException(nameof(port), $"{nameof(port)} must be between 0 and 65535");

            if (string.IsNullOrEmpty(host))
                throw new ArgumentException("Value cannot be null or empty.", nameof(host));

            
            cancellationToken.ThrowIfCancellationRequested();
            
            IPAddress[] addresses = await Dns.GetHostAddressesAsync(host);
            
            cancellationToken.ThrowIfCancellationRequested();

            if (addresses.Length == 0)
                throw new ArgumentException(
                    $"Hostname '{nameof(host)}' must have at least one address in `System.Net.Dns.GetHostAddresses`");

            TcpClient tcpClient = new TcpClient(ProxyHost, ProxyPort);

            CancellationTokenRegistration registration = cancellationToken.Register(() => tcpClient.Close());
                
            try
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await tcpClient.ConnectAsync(host, port).WithCancellationToken(cancellationToken);
                }
                catch (OperationCanceledException ex)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new SocksConnectingException(
                        $"Error when connecting to {host}:{port} through socks4 proxy {ProxyHost}:{ProxyPort}", ex);
                }

                try
                {
                    NetworkStream stream = tcpClient.GetStream();

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        memoryStream.WriteByte(0x04); // Version SOCKS 4
                        memoryStream.WriteByte(0x01); // CONNECT command

                        // Reverse port bytes
                        memoryStream.Write(BitConverter.GetBytes((ushort) port).Reverse().ToArray());

                        // TODO: add support for other addresses
                        memoryStream.Write(addresses[0].GetAddressBytes());

                        // Use `Guid.NewGuid()` as userId
                        memoryStream.Write(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                        memoryStream.WriteByte(0x00);

                        await stream.WriteAsync(memoryStream.GetBuffer(), 0, (int) memoryStream.Position,
                            cancellationToken);
                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    byte[] buffer = new byte[8];

                    int readLength = await stream.ReadAsync(buffer, 0, 8, cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    // TODO: handle another buffer length
                    if (readLength != 8)
                        throw new SocksUnknownException(
                            $"SOCKS4 proxy server returned {readLength} bytes. Expected 8 bytes");

                    byte replyCode = buffer[1];

                    if (replyCode == 90)
                        return tcpClient;

                    throw new SocksResponseException(replyCode);
                }
                catch (OperationCanceledException ex)
                {
                    throw;
                }
                catch (SocksException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    throw new SocksUnknownException("Unknown exception", ex);
                }
            }
            finally
            {
                registration.Dispose();
            }
        }
    }
}
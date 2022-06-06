using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ProxySharp.Tcp.ConnectionProviders.Abstractions;

namespace ProxySharp.Tcp.Extensions
{
    public static class ConnectionProviderExtensions
    {
        public static async Task<TcpClient> Connect(this IConnectionProvider connectionProvider, IPEndPoint endPoint, CancellationToken cancellationToken = default)
        {
            return await connectionProvider.Connect(endPoint.Address.ToString(), endPoint.Port, cancellationToken);
        }
    }
}
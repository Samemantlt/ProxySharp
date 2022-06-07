using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ProxySharp.Tcp.ConnectionProviders.Abstractions
{
    public interface IConnectionProvider
    {
        Task<TcpClient> Connect(string host, int port, CancellationToken cancellationToken = default);
    }
}

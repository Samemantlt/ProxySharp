using System;
using System.Runtime.Serialization;
using ProxySharp.Tcp.ConnectionProviders.Abstractions;

namespace ProxySharp.Tcp.ConnectionProviders.Socks4.Exceptions
{
    [Serializable]
    public class SocksException : ProxyException
    {
        public SocksException(string message) : base(message)
        {
        }

        public SocksException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SocksException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
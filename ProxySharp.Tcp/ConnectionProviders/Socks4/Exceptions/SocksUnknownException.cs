using System;
using System.Runtime.Serialization;

namespace ProxySharp.Tcp.ConnectionProviders.Socks4.Exceptions
{
    [Serializable]
    public class SocksUnknownException : SocksException
    {
        public SocksUnknownException(string message) : base(message)
        {
        }

        public SocksUnknownException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SocksUnknownException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
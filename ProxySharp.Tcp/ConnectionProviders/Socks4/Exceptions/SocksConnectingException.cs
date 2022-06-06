using System;
using System.Runtime.Serialization;

namespace ProxySharp.Tcp.ConnectionProviders.Socks4.Exceptions
{
    [Serializable]
    public class SocksConnectingException : SocksException
    {
        public SocksConnectingException(string message) : base(message)
        {
        }

        public SocksConnectingException(string message, Exception inner) : base(message, inner)
        {
        }

        protected SocksConnectingException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
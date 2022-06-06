using System;
using System.Runtime.Serialization;

namespace ProxySharp.Tcp.ConnectionProviders.Socks4.Exceptions
{
    [Serializable]
    public class SocksResponseException : SocksException
    {
        public int ReplyCode { get; set; }
        
        
        public SocksResponseException(int replyCode) : base($"Reply code is: {replyCode}")
        {
            ReplyCode = replyCode;
        }

        public SocksResponseException(string message, int replyCode) : base(message)
        {
            ReplyCode = replyCode;
        }

        public SocksResponseException(string message, Exception inner, int replyCode) : base(message, inner)
        {
            ReplyCode = replyCode;
        }

        
        protected SocksResponseException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
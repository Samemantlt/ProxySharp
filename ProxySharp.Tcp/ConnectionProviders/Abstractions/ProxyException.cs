using System;
using System.IO;
using System.Runtime.Serialization;

namespace ProxySharp.Tcp.ConnectionProviders.Abstractions
{
    [Serializable]
    public class ProxyException : IOException
    {
        public ProxyException()
        {
        }

        public ProxyException(string message) : base(message)
        {
        }

        public ProxyException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ProxyException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}
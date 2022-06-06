using System.IO;

namespace ProxySharp.Tcp.Extensions
{
    internal static class StreamExtensions
    {
        public static void Write(this Stream stream, byte[] bytes)
        {
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
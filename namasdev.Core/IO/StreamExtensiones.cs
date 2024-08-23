using System.IO;

using namasdev.Core.Validation;

namespace namasdev.Core.IO
{
    public static class StreamExtensiones
    {
        public static byte[] ReadAllBytes(this Stream stream)
        {
            Validador.ValidarArgumentRequeridoYThrow(stream, nameof(stream));
            
            int length = (int)stream.Length;
            var bytes = new byte[length];
            stream.Read(bytes, 0, length);

            return bytes;
        }
    }
}

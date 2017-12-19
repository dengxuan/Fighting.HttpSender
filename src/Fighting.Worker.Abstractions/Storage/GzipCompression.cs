using Fighting.Worker.Storage.Abstractions;
using System.IO;
using System.IO.Compression;

namespace Fighting.Worker.Storage
{
    internal class GzipCompression : IWorkerStorageCompression
    {
        public const int BufferSize = 64 * 1024;

        public virtual byte[] CompressBytes(byte[] bytes)
        {
            using (var compressed = new MemoryStream())
            {
                using (var gzip = new GZipStream(compressed, CompressionMode.Compress))
                using (var gzipStream = new BufferedStream(gzip, BufferSize))
                    gzipStream.Write(bytes, 0, bytes.Length);
                return compressed.ToArray();
            }
        }

        public virtual byte[] DecompressBytes(byte[] bytes)
        {
            using (var compressed = new MemoryStream(bytes))
            using (var decompressed = new MemoryStream())
            {
                using (var gzip = new GZipStream(compressed, CompressionMode.Decompress))
                using (var gzipStream = new BufferedStream(gzip, BufferSize))
                    gzipStream.CopyTo(decompressed);
                return decompressed.ToArray();
            }
        }

    }
}

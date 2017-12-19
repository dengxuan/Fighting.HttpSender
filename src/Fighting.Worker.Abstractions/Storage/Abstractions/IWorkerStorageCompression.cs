namespace Fighting.Worker.Storage.Abstractions
{
    public interface IWorkerStorageCompression
    {
        byte[] CompressBytes(byte[] bytes);
        byte[] DecompressBytes(byte[] bytes);
    }
}
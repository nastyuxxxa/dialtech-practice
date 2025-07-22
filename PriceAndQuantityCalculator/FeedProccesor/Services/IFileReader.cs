namespace FeedProcessor.Services
{
    public interface IFileReader
    {
        Task<string> ReadFileAsync(string filePath);
    }
}

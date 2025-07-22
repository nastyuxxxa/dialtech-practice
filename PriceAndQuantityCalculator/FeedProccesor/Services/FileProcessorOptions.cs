namespace FeedProcessor.Services
{
    public class FileProcessorOptions()
    {
        public string WatchDirectoryPath { get; set; } = Path.Combine(AppContext.BaseDirectory, "feeds");
        public string FileFilter { get; set; } = "*";
        public int MaxParseAttempts { get; set; } = 3;
        public int FileReadRetryDelayMs { get; set; } = 500;
    }
}

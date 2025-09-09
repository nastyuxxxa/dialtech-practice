using Microsoft.Extensions.Logging;

namespace FeedProcessor.Services
{
    public class FileReader(
        ILogger<FileReader> logger,
        int maxAttempts = 3,
        int retryDelayMs = 500) : IFileReader
    {
        private readonly ILogger<FileReader> _logger = logger;
        private readonly int _maxAttempts = maxAttempts;
        private readonly int _retryDelayMs = retryDelayMs;

        public async Task<string> ReadFileAsync(string filePath)
        {
            for (int attempt = 1; attempt <= _maxAttempts; attempt++)
            {
                try
                {
                    return await File.ReadAllTextAsync(filePath);
                }
                catch (IOException ex) when (IsFileLocked(ex))
                {
                    if (attempt == _maxAttempts)
                    {
                        _logger.LogError(ex, "Не удалось прочитать файл {filePath} после {maxAttempts} попыток.", filePath, _maxAttempts);
                        throw;
                    }

                    await Task.Delay(_retryDelayMs);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при чтении файла {filePath}.", filePath);
                    throw;
                }
            }

            throw new InvalidOperationException($"Непредвиденная ошибка: чтение файла {filePath} завершилось без возврата значения");
        }

        private static bool IsFileLocked(IOException ex)
            => ex.Message.Contains("because it is being used by another process");
    }
}

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMqServices.Services;

namespace FeedProcessor.Services
{
    public class FeedWatcher(
        ILogger<FeedWatcher> logger,
        IFileReader fileReader,
        IFeedParser parser,
        IRabbitMqPublisher publisher,
        IOptions<FileProcessorOptions> options) : BackgroundService, IFileWatcher
    {
        private readonly ILogger<FeedWatcher> _logger = logger;
        private readonly IFileReader _fileReader = fileReader;
        private readonly IFeedParser _parser = parser;
        private readonly IRabbitMqPublisher _publisher = publisher;
        private FileSystemWatcher _fileWatcher;
        private readonly FileProcessorOptions _options = options.Value;
        private readonly SemaphoreSlim _processingLock = new(1, 1);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!Directory.Exists(_options.WatchDirectoryPath))
            {
                Directory.CreateDirectory(_options.WatchDirectoryPath);
                _logger.LogInformation("Создана директория {directory}.", _options.WatchDirectoryPath);
            }

            _fileWatcher = new FileSystemWatcher(_options.WatchDirectoryPath, _options.FileFilter)
            {
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.CreationTime
            };

            _fileWatcher.Created += async (s, e) => await OnFileCreatedAsync(e.FullPath, e.Name, stoppingToken);
            _fileWatcher.EnableRaisingEvents = true;

            _logger.LogInformation("FeedWatcher запущен над директорией: {directory}.", _options.WatchDirectoryPath);

            return Task.CompletedTask;
        }

        private async Task OnFileCreatedAsync(string filePath, string fileName, CancellationToken cancellationToken)
        {
            await _processingLock.WaitAsync(cancellationToken);

            try
            {
                var feedContent = await _fileReader.ReadFileAsync(filePath);
                var products = _parser.Parse(feedContent);

                if (products != null && products.Count > 0)
                {
                    foreach (var product in products)
                    { 
                        await _publisher.PublishProductAsync(product);
                    }
                    _logger.LogInformation("Файл {fileName} успешно обработан.", fileName);
                }
                else
                {
                    _logger.LogWarning("Файл {fileName} не содержит валидных продуктов.", fileName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке файла {fileName}.", fileName);
            }
            finally
            {
                _processingLock.Release();
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _fileWatcher?.Dispose();
            _logger.LogInformation("FeedWatcher остановлен.");
            return base.StopAsync(cancellationToken);
        }
    }
}

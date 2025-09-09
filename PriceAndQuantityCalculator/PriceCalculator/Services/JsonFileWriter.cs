using Data.Models;
using Microsoft.Extensions.Logging;
using PriceCalculation.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PriceCalculation.Services
{
    public class JsonFileWriter(
        ILogger<JsonFileWriter> logger) : IFileWriter
    {
        private readonly ILogger<JsonFileWriter> _logger = logger;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        public async Task WriteToFileAsync(Product product, string directoryPath)
        {
            try
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                    _logger.LogInformation("Создана директория {directory}.", directoryPath);
                }

                var fileName = $"{product.Id}_{DateTime.Now:dd-MM-yyyy_HH-mm-ss}.json";
                var filePath = Path.Combine(directoryPath, fileName);
                var productDto = new ProductDto(product);
                var json = JsonSerializer.Serialize(productDto, _jsonOptions);

                await File.WriteAllTextAsync(filePath, json);
                _logger.LogInformation("Результат калькуляции количества и цен продукта записан в файл {fileName}", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при записи результата калькуляции количества и цен продукта {id} в файл.", product.Id);
            }
        }
    }
}

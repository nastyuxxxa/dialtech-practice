using Microsoft.Extensions.Logging;
using Data.Models;
using System.Text.Json;

namespace FeedProcessor.Services
{
    public class JsonFeedParser(
        ILogger<JsonFeedParser> logger) : IFeedParser
    {
        private readonly ILogger<JsonFeedParser> _logger = logger;

        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true,
        };

        public List<Product> Parse(string feedContent)
        {
            if (string.IsNullOrWhiteSpace(feedContent))
            {
                _logger.LogWarning("Получен пустой контент для парсинга.");
                return [];
            }

            try
            {
                using var jsonDocument = JsonDocument.Parse(feedContent);
                var products = new List<Product>();

                foreach (var element in jsonDocument.RootElement.EnumerateArray())
                {
                    try
                    {
                        var product = element.Deserialize<Product>(_options);
                        if (product is null || string.IsNullOrWhiteSpace(product.Type))
                        {
                            _logger.LogWarning("Один из продуктов невалиден: отсутствует тип или объект null.");
                        }
                        else
                        {
                            products.Add(product);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Ошибка при парсинге одного из продуктов.");
                    }
                }

                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при парсинге JSON.");
                return [];
            }
        }
    }
}
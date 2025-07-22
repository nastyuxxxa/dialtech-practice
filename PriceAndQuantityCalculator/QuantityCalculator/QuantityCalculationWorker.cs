using Microsoft.Extensions.Logging;
using QuantityCalculation.Services;
using RabbitMqServices.Services;

namespace QuantityCalculation
{
    public class QuantityCalculationWorker(
        ILogger<QuantityCalculationWorker> logger,
        IRabbitMqListener listener,
        IRabbitMqPublisher publisher,
        IQuantityCalculator calculator) : IQuantityCalculationWorker
    {
        private readonly ILogger<QuantityCalculationWorker> _logger = logger;
        private readonly IRabbitMqListener _listener = listener;
        private readonly IRabbitMqPublisher _publisher = publisher;
        private readonly IQuantityCalculator _calculator = calculator;

        public void Run()
        {
             _listener.ProductReceived += async (product) =>
            {
                try
                {
                    var calculatedProduct = _calculator.Calculate(product);
                    await _publisher.PublishProductAsync(calculatedProduct);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при калькуляции количества продукта {Id}", product.Id);
                }
            };
        }
    }
}

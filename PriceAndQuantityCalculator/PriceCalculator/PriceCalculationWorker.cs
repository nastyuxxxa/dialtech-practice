using Microsoft.Extensions.Options;
using PriceCalculation.Services;
using RabbitMqServices.Services;

namespace PriceCalculation
{
    public class PriceCalculationWorker(
        IRabbitMqListener listener,
        IPriceCalculator calculator,
        IFileWriter fileWriter,
        IOptions<PriceCalculationOptions> options) : IPriceCalculationWorker
    {
        private readonly IRabbitMqListener _listener = listener;
        private readonly IPriceCalculator _calculator = calculator;
        private readonly IFileWriter _fileWriter = fileWriter;
        private readonly string _directoryPath = options.Value.ResultDirectoryPath;

        public void Run()
        {
            _listener.ProductReceived += async (product) =>
            {
                var calculatedProduct = _calculator.Calculate(product);

                await _fileWriter.WriteToFileAsync(calculatedProduct, _directoryPath);
            };
        }
    }
}

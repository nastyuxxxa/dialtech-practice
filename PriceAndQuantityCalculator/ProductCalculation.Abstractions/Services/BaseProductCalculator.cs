using Data.Models;
using Microsoft.Extensions.Logging;

namespace ProductCalculation.Abstractions.Services
{
    public abstract class BaseProductCalculator<T>(
        IEnumerable<IProductCalculationStrategy> strategies,
        ILogger<T> logger) : IProductCalculator
    {
        private readonly IEnumerable<IProductCalculationStrategy> _strategies = strategies;
        private readonly ILogger<T> _logger = logger;

        public Product Calculate(Product product)
        {
            try
            {
                var strategy = _strategies.FirstOrDefault(strategy => strategy.CanCalculate(product.Type));
                if ( strategy is null)
                {
                    throw new NotSupportedException($"Неподдерживаемый тип продукта: {product.Type}");
                }                    

                strategy.Calculate(product);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}

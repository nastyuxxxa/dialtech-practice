using Microsoft.Extensions.Logging;
using ProductCalculation.Abstractions.Services;

namespace QuantityCalculation.Services
{
    public class QuantityCalculator(
        IEnumerable<IProductCalculationStrategy> strategies,
        ILogger<QuantityCalculator> logger) : BaseProductCalculator<QuantityCalculator>(strategies, logger), IQuantityCalculator
    {
    }
}

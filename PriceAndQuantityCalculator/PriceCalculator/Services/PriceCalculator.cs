using Microsoft.Extensions.Logging;
using ProductCalculation.Abstractions.Services;

namespace PriceCalculation.Services
{
    public class PriceCalculator(
        IEnumerable<IProductCalculationStrategy> strategies,
        ILogger<PriceCalculator> logger) : BaseProductCalculator<PriceCalculator>(strategies, logger), IPriceCalculator
    {
    }
}

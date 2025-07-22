using ProductCalculation.Abstractions.Services;
using Microsoft.Extensions.DependencyInjection;
using QuantityCalculation.Services;
using QuantityCalculation.Strategies;

namespace QuantityCalculation.Configuration
{
    public static class QuantityCalculationConfigurator
    {
        public static void AddQuantityCalculationServices(this IServiceCollection services)
        {
            services.AddSingleton<IProductCalculationStrategy, ProductQuantityStrategy>();
            services.AddSingleton<IProductCalculationStrategy, SetOrVariantQuantityStrategy>();
            services.AddSingleton<IQuantityCalculator, QuantityCalculator>();
            services.AddSingleton<IQuantityCalculationWorker, QuantityCalculationWorker>();
        }
    }
}

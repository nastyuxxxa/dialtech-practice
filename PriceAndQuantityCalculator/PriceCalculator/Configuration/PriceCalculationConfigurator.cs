using Microsoft.Extensions.DependencyInjection;
using PriceCalculation.Services;
using PriceCalculation.Strategies;
using ProductCalculation.Abstractions.Services;

namespace PriceCalculation.Configuration
{
    public static class PriceCalculationConfigurator
    {
        public static void AddPriceCalculationServices(this IServiceCollection services, Action<PriceCalculationOptions> configureOptions)
        {
            services.Configure(configureOptions);

            services.AddSingleton<IProductCalculationStrategy, ProductPriceStrategy>();
            services.AddSingleton<IProductCalculationStrategy, SetOrVariantPriceStrategy>();
            services.AddSingleton<IPriceCalculator, PriceCalculator>();
            services.AddSingleton<IFileWriter, JsonFileWriter>();
            services.AddSingleton<IPriceCalculationWorker, PriceCalculationWorker>();
        }
    }
}

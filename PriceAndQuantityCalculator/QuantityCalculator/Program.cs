using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using QuantityCalculation.Configuration;
using RabbitMqServices.Configuration;

namespace QuantityCalculation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();

            host.Services.GetRequiredService<IQuantityCalculationWorker>().Run();

            await host.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddQuantityCalculationServices();
            services.AddRabbitMqListenerService("quantity_calculation_queue");
            services.AddRabbitMqPublisherService("price_calculation_queue");
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddNLog("nlog.config");
            });
        }
    }
}
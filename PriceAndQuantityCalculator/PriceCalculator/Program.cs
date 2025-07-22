using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using PriceCalculation.Configuration;
using RabbitMqServices.Configuration;

namespace PriceCalculation
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services, context.Configuration);
                })
                .Build();

            host.Services.GetRequiredService<IPriceCalculationWorker>().Run();

            await host.RunAsync();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddPriceCalculationServices(options =>
            {
                options.ResultDirectoryPath = "C:\\Users\\User\\source\\repos\\dialtech-practice\\PriceAndQuantityCalculator\\Results";
            });
            services.AddRabbitMqListenerService("price_calculation_queue");
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddNLog("nlog.config");
            });
        }
    }
}
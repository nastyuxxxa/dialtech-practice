using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMqServices.Configuration;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using FeedProcessor.Configuration;

namespace FeedProcessor
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

            await host.RunAsync();  
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddFeedProcessorServices(options =>
            {
                options.WatchDirectoryPath = "C:\\test";
                options.FileFilter = "*.json";
            });
            services.AddRabbitMqPublisherService("quantity_calculation_queue");
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddNLog("nlog.config");
            });
        }
    }
}
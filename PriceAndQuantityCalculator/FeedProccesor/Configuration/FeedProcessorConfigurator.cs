using FeedProcessor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FeedProcessor.Configuration
{
    public static class FeedProcessorConfigurator
    {
        public static void AddFeedProcessorServices(this IServiceCollection services, Action<FileProcessorOptions> configureOptions)
        {
            services.Configure(configureOptions);

            services.AddSingleton<IFileReader, FileReader>();
            services.AddSingleton<IFeedParser, JsonFeedParser>();
            services.AddSingleton<IFileWatcher, FeedWatcher>();
            services.AddSingleton<IHostedService>(serviceProvider => serviceProvider.GetRequiredService<IFileWatcher>());
        }
    }
}


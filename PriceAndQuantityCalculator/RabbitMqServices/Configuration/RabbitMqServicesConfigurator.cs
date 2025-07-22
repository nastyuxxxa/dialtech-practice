using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMqServices.Services;
using System;

namespace RabbitMqServices.Configuration
{
    static public class RabbitMqServicesConfigurator
    {
        public static void AddRabbitMqListenerService(this IServiceCollection services, string queueName)
        {
            services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddSingleton<IRabbitMqListener, RabbitMqListener>(serviceProvider
                => new RabbitMqListener(queueName,
                serviceProvider.GetRequiredService<ILogger<RabbitMqListener>>(),
                serviceProvider.GetRequiredService<IRabbitMqConnectionFactory>()));
            services.AddSingleton<IHostedService>(serviceProvider => serviceProvider.GetRequiredService<IRabbitMqListener>());
        }

        public static void AddRabbitMqPublisherService(this IServiceCollection services, string queueName)
        {
            services.AddSingleton<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>(serviceProvider
                => new RabbitMqPublisher(queueName,
                serviceProvider.GetRequiredService<ILogger<RabbitMqPublisher>>(),
                serviceProvider.GetRequiredService<IRabbitMqConnectionFactory>()));
        }
    }
}

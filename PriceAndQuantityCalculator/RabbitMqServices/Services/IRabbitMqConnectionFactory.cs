using RabbitMQ.Client;

namespace RabbitMqServices.Services
{
    public interface IRabbitMqConnectionFactory
    {
        Task<IChannel> ConnectAsync(string queueName, CancellationToken cancellationToken = default);
        Task DisconnectAsync();
    }
}

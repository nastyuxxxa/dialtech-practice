using RabbitMQ.Client;

namespace RabbitMqServices.Services
{
    public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
    {
        private IConnection _connection;
        private IChannel _channel;

        public async Task<IChannel> ConnectAsync(string queueName, CancellationToken cancellationToken = default)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

            return _channel;
        }

        public async Task DisconnectAsync()
        {
            if (_channel != null)
                await _channel.DisposeAsync();
            if (_connection != null)
                await _connection.DisposeAsync();
        }
    }
}

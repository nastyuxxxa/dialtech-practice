using Microsoft.Extensions.Logging;
using Data.Models;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace RabbitMqServices.Services
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly ILogger<RabbitMqPublisher> _logger;
        private readonly IRabbitMqConnectionFactory _connectionFactory;
        private IChannel _channel;
        private readonly string _queueName;
        private bool _initialized = false;

        public RabbitMqPublisher(string queueName, ILogger<RabbitMqPublisher> logger, IRabbitMqConnectionFactory connectionFactory)
        {
            _logger = logger;
            _queueName = queueName;
            _connectionFactory = connectionFactory;
        }

        private async Task InitializeAsync()
        {
            try
            {
                _channel = await _connectionFactory.ConnectAsync(_queueName);
                _initialized = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка инициализации RabbitMQ Publisher.");
            }
        }

        public async Task PublishProductAsync(Product product)
        {
            if (!_initialized)
            {
                await InitializeAsync();
            }

            try
            {
                var message = JsonSerializer.Serialize(product);
                var body = Encoding.UTF8.GetBytes(message);

                await _channel.BasicPublishAsync(exchange: "",
                                        routingKey: _queueName,
                                        body: body);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке сообщения с продуктом {id} в очередь {queue}.", product.Id, _queueName);
            }
        }
    }
}

using Microsoft.Extensions.Logging;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Data.Models;

namespace RabbitMqServices.Services
{
    public class RabbitMqListener : BackgroundService, IRabbitMqListener
    {
        private readonly ILogger<RabbitMqListener> _logger;
        private readonly IRabbitMqConnectionFactory _connectionFactory;
        private IChannel _channel;
        private readonly string _queueName;

        public event Func<Product, Task> ProductReceived;

        public RabbitMqListener(string queueName, ILogger<RabbitMqListener> logger, IRabbitMqConnectionFactory connectionFactory)
        {
            _logger = logger;
            _queueName = queueName;
            _connectionFactory = connectionFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _channel = await _connectionFactory.ConnectAsync(_queueName, stoppingToken);

                stoppingToken.ThrowIfCancellationRequested();

                var consumer = new AsyncEventingBasicConsumer(_channel);

                consumer.ReceivedAsync += OnMessageReceived;

                await _channel.BasicConsumeAsync(_queueName, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

                _logger.LogInformation("Подключение к RabbitMQ и подписка на очередь {queue}.", _queueName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка инициализации RabbitMQ Listener.");
            }
        }

        private async Task OnMessageReceived(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                var product = JsonSerializer.Deserialize<Product>(message);

                if (product == null)
                {
                    _logger.LogWarning("Получено пустое или некорректное сообщение.");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, false);
                    return;
                }

                if (ProductReceived != null)
                    await ProductReceived(product);

                await _channel.BasicAckAsync(ea.DeliveryTag, false);

                _logger.LogInformation("Сообщение из очереди {queue} с продуктом {productId} обработано.", _queueName, product.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке сообщения.");
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connectionFactory.DisconnectAsync();
            
            _logger.LogInformation("RabbitMqListener для очереди {queue} остановлен.", _queueName);

            await base.StopAsync(cancellationToken);
        }
    }
}

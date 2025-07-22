using Data.Models;

namespace RabbitMqServices.Services
{
    public interface IRabbitMqPublisher
    {
        Task PublishProductAsync(Product product);
    }
}

using Data.Models;
using Microsoft.Extensions.Hosting;

namespace RabbitMqServices.Services
{
    public interface IRabbitMqListener : IHostedService
    {
        public event Func<Product, Task> ProductReceived;
    }
}

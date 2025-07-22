using Data.Models;

namespace ProductCalculation.Abstractions.Services
{
    public interface IProductCalculator
    {
        Product Calculate(Product product);
    }
}

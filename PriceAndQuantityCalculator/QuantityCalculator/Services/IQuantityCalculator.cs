using Data.Models;

namespace QuantityCalculation.Services
{
    public interface IQuantityCalculator
    {
        Product Calculate(Product product);
    }
}

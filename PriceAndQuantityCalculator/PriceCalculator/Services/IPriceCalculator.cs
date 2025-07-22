using Data.Models;

namespace PriceCalculation.Services
{
    public interface IPriceCalculator
    {
        Product Calculate(Product product);
    }
}

using Data.Models;

namespace ProductCalculation.Abstractions.Services
{
    public interface IProductCalculationStrategy
    {
        bool CanCalculate(string productType);
        void Calculate(Product product);
    }
}

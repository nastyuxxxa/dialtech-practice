using Data.Models;
using ProductCalculation.Abstractions.Services;

namespace PriceCalculation.Strategies
{
    public class ProductPriceStrategy : IProductCalculationStrategy
    {
        public bool CanCalculate(string productType) => productType == "product";

        public void Calculate(Product product)
        {
            product.WarehousePrice = product.Warehouses?.Average(w => w.Price) ?? 0;
            product.SupplierPrice = product.Suppliers?.Min(s => s.Price) ?? 0;
            product.MinPrice = CalculateMinPrice(product.WarehousePrice, product.SupplierPrice);
        }

        private static double CalculateMinPrice(params double[] prices)
            => prices.Where(p => p > 0).DefaultIfEmpty(0).Min();
    }
}

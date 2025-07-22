using Data.Models;
using ProductCalculation.Abstractions.Services;

namespace PriceCalculation.Strategies
{
    public class SetOrVariantPriceStrategy : IProductCalculationStrategy
    {
        public bool CanCalculate(string productType) => productType == "set" || productType == "variant";

        public void Calculate(Product product)
        {
            product.SupplierPrice = 0;

            if (product.SubProducts == null || product.SubProducts.Count == 0)
            {
                product.WarehousePrice = 0;
                product.MinPrice = 0;
                return;
            }

            var productPriceStrategy = new ProductPriceStrategy();
            foreach (var subProduct in product.SubProducts)
            {
                productPriceStrategy.Calculate(subProduct);
            }

            product.WarehousePrice = product.Type == "set"
                ? product.SubProducts.Sum(sp => sp.WarehousePrice)
                : product.SubProducts.Min(sp => sp.SupplierPrice);
            product.MinPrice = product.WarehousePrice;
        }
    }
}

using Data.Models;
using ProductCalculation.Abstractions.Services;

namespace QuantityCalculation.Strategies
{
    public class SetOrVariantQuantityStrategy : IProductCalculationStrategy
    {
        public bool CanCalculate(string productType) => productType == "set" || productType == "variant";

        public void Calculate(Product product)
        {
            product.SupplierQuantity = 0;

            if (product.SubProducts == null || product.SubProducts.Count == 0)
            {
                product.WarehouseQuantity = 0;
                product.Quantity = 0;
                return;
            }

            var productQuantityStrategy = new ProductQuantityStrategy();


            foreach (var subProduct in product.SubProducts)
            {
                productQuantityStrategy.Calculate(subProduct);
            }

            product.WarehouseQuantity = product.SubProducts.Min(sp => sp.WarehouseQuantity);
            product.Quantity = product.WarehouseQuantity;
        }
    }
}

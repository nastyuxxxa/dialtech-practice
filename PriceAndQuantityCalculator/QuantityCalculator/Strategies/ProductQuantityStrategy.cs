using Data.Models;
using ProductCalculation.Abstractions.Services;

namespace QuantityCalculation.Strategies
{
    public class ProductQuantityStrategy : IProductCalculationStrategy
    {
        public bool CanCalculate(string productType) => productType == "product";

        public void Calculate(Product product)
        {
            product.WarehouseQuantity = product.Warehouses?.Sum(w => w.Quantity) ?? 0;
            product.SupplierQuantity = product.Suppliers?.Sum(s => s.Quantity) ?? 0;
            product.Quantity = product.WarehouseQuantity + product.SupplierQuantity;
        }
    }
}

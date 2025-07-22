using Data.Models;

namespace PriceCalculation.Models
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public int WarehouseQuantity { get; set; }
        public int SupplierQuantity { get; set; }
        public int Quantity { get; set; }
        public double WarehousePrice { get; set; }
        public double SupplierPrice { get; set; }
        public double MinPrice { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<SubProductDto> SubProducts { get; set; }
        public List<Warehouse> Warehouses { get; set; }

        public ProductDto(Product product)
        {
            Id = product.Id;
            Type = product.Type;
            WarehouseQuantity = product.WarehouseQuantity;
            SupplierQuantity = product.SupplierQuantity;
            Quantity = product.Quantity;
            WarehousePrice = product.WarehousePrice;
            SupplierPrice = product.SupplierPrice;
            MinPrice = product.MinPrice;
            Suppliers = product.Suppliers;
            SubProducts = product.SubProducts?
                .Select(subProduct => new SubProductDto
                {
                    Id = subProduct.Id,
                    Suppliers = subProduct.Suppliers,
                    Warehouses = subProduct.Warehouses
                })
                .ToList();
            Warehouses = product.Warehouses;
        }
    }
}

namespace Data.Models
{
    public class Product
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
        public List<SubProduct> SubProducts { get; set; }
        public List<Warehouse> Warehouses { get; set; }
    }
}

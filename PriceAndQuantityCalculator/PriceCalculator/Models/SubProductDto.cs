using Data.Models;

namespace PriceCalculation.Models
{
    public class SubProductDto
    {
        public Guid Id { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public List<Warehouse> Warehouses { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ApplianceShop.Entities.Parts;

namespace ApplianceShop.Entities.Appliances
{
    public class Appliance
    {
        public int Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }

        [Column(TypeName = "decimal(6, 2)")] // Precision(p) - total number (6) ; Scale (s) - digits (2) after decimal point 
        public decimal Price { get; set; }
        public string? Features { get; set; }

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        public string? ImageName { get; set; }

        public List<Part>? Parts { get; set; } = [];
    }
}

using ApplianceShop.Entities.Parts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplianceShop.Models.Appliances
{
    public class UpdateApplianceViewModel
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }

        [Column(TypeName = "decimal(7, 2)")] // Precision(p) - total number (7) ; Scale (s) - digits (2) after decimal point 
        public decimal Price { get; set; }
        public string? Features { get; set; }

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        public string? ImageName { get; set; }
    }
}

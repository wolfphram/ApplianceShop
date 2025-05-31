using ApplianceShop.Entities.Appliances;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplianceShop.Models.Parts
{
    public class PartDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Part Number")]
        public string PartNumber { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Price { get; set; }

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        public List<Appliance> Appliances { get; set; }
    }
}

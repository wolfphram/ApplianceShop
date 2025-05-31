using ApplianceShop.Entities.Appliances;
using ApplianceShop.Entities.Customers;
using ApplianceShop.Entities.Parts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ApplianceShop.Utils.Enums;
using ApplianceShop.Models.Appliances;
using ApplianceShop.Models.Parts;

namespace ApplianceShop.Models.Orders
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Order Time")]
        public DateTime OrderTime { get; set; }
        [Display(Name = "Total Price")]
        [Column(TypeName = "decimal(5,2)")]
        public decimal TotalPrice { get; set; }
        public string Notes { get; set; }

        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Paid")]
        public bool IsPaid { get; set; }

        public int CustomerId { get; set; }
        public List<ApplianceViewModel> Appliances { get; set; } = [];
        public List<PartViewModel>? Parts { get; set; } = [];

        public string? CustomerFullName { get; set; }
    }
}

using ApplianceShop.Entities.Appliances;
using ApplianceShop.Entities.Customers;
using ApplianceShop.Entities.Parts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ApplianceShop.Utils.Enums;

namespace ApplianceShop.Models.Orders
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Order Time")]
        public DateTime OrderTime { get; set; }
        [Display(Name = "Total Price")]
        [Column(TypeName = "decimal(7,2)")]
        public decimal TotalPrice { get; set; }
        public string Notes { get; set; }

        [Display(Name = "Payment Method")]
        public PaymentMethod PaymentMethod { get; set; }

        [Display(Name = "Paid")]
        public bool IsPaid { get; set; }

        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public List<Appliance> Appliances { get; set; }
        public List<Part>? Parts { get; set; }
    }
}

using ApplianceShop.Entities.Appliances;
using ApplianceShop.Entities.Customers;
using ApplianceShop.Entities.Parts;
using ApplianceShop.Utils.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplianceShop.Entities.Orders
{
    public class Order
    {
        public int Id { get; set; }

        [Display(Name = "Order Time")]
        public DateTime OrderTime { get; set; }
        [Display(Name ="Total Price")]
        [Column(TypeName ="decimal(7,2)")]
        public decimal TotalPrice { get; set; }
        public string? Notes { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public List<Appliance> Appliances { get; set; }
        public List<Part>? Parts { get; set; }
        public bool IsPaid { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}

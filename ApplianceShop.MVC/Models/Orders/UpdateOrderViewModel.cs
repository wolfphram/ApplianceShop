using ApplianceShop.Entities.Appliances;
using ApplianceShop.Entities.Customers;
using ApplianceShop.Entities.Parts;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ApplianceShop.Utils.Enums;

namespace ApplianceShop.Models.Orders
{
    public class UpdateOrderViewModel
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
        public List<int> ApplianceIds { get; set; }
        public List<int>? PartIds { get; set; }

        public string? CustomerFullName { get; set; }


        [ValidateNever]
        public SelectList CustomerLookup { get; set; }

        [ValidateNever]
        public MultiSelectList ApplianceLookup { get; set; }

        [ValidateNever]
        public MultiSelectList PartLookup { get; set; }
    }
}

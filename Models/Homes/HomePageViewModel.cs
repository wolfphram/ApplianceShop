using ApplianceShop.Entities.Appliances;
using ApplianceShop.Entities.Parts;
using ApplianceShop.Models.Appliances;
using ApplianceShop.Models.Customers;
using ApplianceShop.Models.Parts;

namespace ApplianceShop.Models.Homes
{
    public class HomePageViewModel
    {
        public List<ApplianceViewModel> GetAppliances { get; set; } = [];
        public List<PartViewModel> GetParts { get; set; } = [];
        public List<CustomerViewModel> OurCustomers { get; set; } = [];


    }
}

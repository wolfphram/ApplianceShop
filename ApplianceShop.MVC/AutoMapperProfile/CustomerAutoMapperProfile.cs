using ApplianceShop.Entities.Appliances;
using ApplianceShop.Entities.Customers;
using ApplianceShop.Models.Appliances;
using ApplianceShop.Models.Customers;
using AutoMapper;

namespace ApplianceShop.AutoMapperProfile
{
    public class CustomerAutoMapperProfile : Profile
    {
        public CustomerAutoMapperProfile()
        {
            CreateMap<Customer, CustomerViewModel>();
            CreateMap<Customer, CustomerDetailsViewModel>();
            CreateMap<CreateCustomerViewModel, Customer>().ReverseMap();
            CreateMap<UpdateCustomerViewModel, Customer>().ReverseMap();
        }
    }
}

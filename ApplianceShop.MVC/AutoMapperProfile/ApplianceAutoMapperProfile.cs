using ApplianceShop.Entities.Appliances;
using ApplianceShop.Models.Appliances;
using AutoMapper;

namespace ApplianceShop.AutoMapperProfile
{
    public class ApplianceAutoMapperProfile : Profile
    {
        public ApplianceAutoMapperProfile()
        {
            CreateMap<Appliance, ApplianceViewModel>();
            CreateMap<Appliance, ApplianceDetailsViewModel>();
            CreateMap<CreateApplianceViewModel, Appliance>().ReverseMap();
            CreateMap<UpdateApplianceViewModel, Appliance>().ReverseMap();


        }
    }
}

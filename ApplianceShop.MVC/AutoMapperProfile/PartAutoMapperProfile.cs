using ApplianceShop.Entities.Parts;
using ApplianceShop.Models.Parts;
using AutoMapper;

namespace ApplianceShop.AutoMapperProfile
{
    public class PartAutoMapperProfile : Profile
    {
        public PartAutoMapperProfile()
        {
            CreateMap<Part, PartViewModel>();
            CreateMap<Part, PartDetailsViewModel>();
            CreateMap<CreatePartViewModel, Part>().ReverseMap();
            CreateMap<UpdatePartViewModel, Part>().ReverseMap();

        }
    }
}

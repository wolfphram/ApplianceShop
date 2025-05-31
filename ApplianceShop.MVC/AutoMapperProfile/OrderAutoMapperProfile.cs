using ApplianceShop.Entities.Orders;
using ApplianceShop.Models.Orders;
using AutoMapper;

namespace ApplianceShop.AutoMapperProfile
{
    public class OrderAutoMapperProfile : Profile
    {
        public OrderAutoMapperProfile()
        {
            CreateMap<Order, OrderViewModel>();
            CreateMap<Order, OrderDetailsViewModel>();
            CreateMap<CreateOrderViewModel, Order>().ReverseMap();
            CreateMap<UpdateOrderViewModel, Order>().ReverseMap();
        }
    }
}

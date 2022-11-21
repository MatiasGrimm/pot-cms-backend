using AutoMapper;
using PotShop.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.ViewModels.Mappings
{
    public class EntityToViewModelMappingProfile : Profile
    {
        public EntityToViewModelMappingProfile()
        {
            CreateMap<Staff, SimpleStaffViewModel>();

            CreateMap<Staff, StaffViewModel>()
                .ReverseMap();

            CreateMap<Product, ProductViewModel>()
                .ReverseMap();

            CreateMap<Product, SimpleProductViewModel>();

            CreateMap<InventoryProduct, InventoryProductViewModel>()
                .ReverseMap();

            CreateMap<Inventory, InventoryViewModel>()
                .ReverseMap();

            CreateMap<Inventory, SimpleInventoryViewModel>();

            CreateMap<SalesHistory, SalesHistoryViewModel>()
                .ReverseMap();

            CreateMap<SalesHistory, SimpleSalesHistoryViewModel>();

            CreateMap<Location, LocationViewModel>()
                .ReverseMap()
                .ForMember(x => x.Manager, opt => opt.Ignore());

            CreateMap<Location, SimpleLocationViewModel>();

            CreateMap<IdentityRole, string>()
                .ConvertUsing(x => x.Name);
        }
    }
}

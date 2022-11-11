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
            CreateMap<ApiUser, SimpleEmployeeViewModel>();

            CreateMap<ApiUser, UserViewModel>()
                .ReverseMap();

            CreateMap<Product, ProductViewModel>()
                .ReverseMap();

            CreateMap<Inventory, InventoryViewModel>()
                .ReverseMap();

            CreateMap<SalesHistory, SalesHistoryViewModel>()
                .ReverseMap();
            
            CreateMap<ProductList, ProductListViewModel>()
                .ReverseMap();

            CreateMap<Location, LocationViewModel>()
                .ReverseMap();

            CreateMap<IdentityRole, string>()
                .ConvertUsing(x => x.Name);
        }
    }
}

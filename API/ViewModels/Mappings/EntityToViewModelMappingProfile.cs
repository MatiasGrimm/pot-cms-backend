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
            CreateMap<Company, CompanyViewModel>()
                .ReverseMap();

            CreateMap<Company, CompanyDetailsViewModel>();

            CreateMap<ApiUser, UserViewModel>();

            CreateMap<IdentityRole, string>()
                .ConvertUsing(x => x.Name);
        }
    }
}

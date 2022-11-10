using PotShop.API.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API
{
    public static class ControllerExtensions
    {
        public static bool UserHasAccess(this Controller controller, Guid companyId) { 
            return controller.User.GetLocationId() == companyId || controller.User.IsAdmin();
        }

        //public static bool UserHasAccess(this Controller controller, ICompanyEntity entity)
        //{
        //    return UserHasAccess(controller, entity.CompanyId);
        //}
    }
}

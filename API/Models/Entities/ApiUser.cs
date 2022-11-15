using Microsoft.AspNetCore.Identity;
using PotShop.API.Models.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.Models.Entities
{
    public class ApiUser : IdentityUser, IDatedEntity, IDisabledEntity
    {
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool IsDisabled { get; set; }

        public StaffAccess StaffAccess { get; set; }

        public List<IdentityRole> Roles { get; set; }

        public Location Location { get; set; }

        public List<SalesHistory> SalesHistory { get; set; }
    }
}

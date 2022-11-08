using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.Models.Entities
{
    public class ApiUser : IdentityUser, IDatedEntity, ICompanyEntity, IDisabledEntity
    {
        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        public string Name { get; set; }

        public bool isDisabled { get; set; }

        public Guid CompanyId { get; set; }

        public Company Company { get; set; }

        public List<IdentityRole> Roles { get; set; }
    }
}

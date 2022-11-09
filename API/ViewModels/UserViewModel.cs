using PotShop.API.Models.Entities;
using System;
using System.Collections.Generic;

namespace PotShop.API.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool IsDisabled { get; set; }

        public Guid CompanyId { get; set; }

        public Location Location { get; set; }

        public List<string> Roles { get; set; }
    }
}

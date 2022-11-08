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

        public bool Disabled { get; set; }

        public Guid? CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; }
    }
}

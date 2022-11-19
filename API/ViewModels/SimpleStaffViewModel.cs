using System;

namespace PotShop.API.ViewModels
{
    public class SimpleStaffViewModel
    {
        public Guid id { get; set; }

        public string name { get; set; }

        public int RoleId { get; set; }

        public bool IsDisabled { get; set; }
    }
}

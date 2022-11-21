using PotShop.API.Models.Entities;
using System;

namespace PotShop.API.ViewModels
{
    public class LocationViewModel : SimpleLocationViewModel
    {
        public int Type { get; set; }

        public bool IsDisabled { get; set; }

        public SimpleInventoryViewModel Inventory { get; set; }

        public SimpleStaffViewModel Manager { get; set; }
    }
}

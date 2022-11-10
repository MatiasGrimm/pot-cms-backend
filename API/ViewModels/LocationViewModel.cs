using System;

namespace PotShop.API.ViewModels
{
    public class LocationViewModel : SimpleLocationViewModel
    {
        public int Type { get; set; }

        public bool IsDisabled { get; set; }

        public Guid InventoryId { get; set; }
    }
}

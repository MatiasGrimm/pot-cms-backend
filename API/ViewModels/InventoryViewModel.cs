using System;

namespace PotShop.API.ViewModels
{
    public class InventoryViewModel : SimpleInventoryViewModel
    {
        public Guid Id { get; set; }
        public Guid? locationId { get; set; }
    }
}

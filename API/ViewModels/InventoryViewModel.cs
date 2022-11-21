using System;
using System.Collections.Generic;

namespace PotShop.API.ViewModels
{
    public class InventoryViewModel : SimpleInventoryViewModel
    {
        public Guid Id { get; set; }

        public List<InventoryProductViewModel> Products { get; set; }
    }
}

using System;

namespace PotShop.API.ViewModels
{
    public class InventoryProductViewModel
    {
        public Guid Id { get; set; }

        public int Quantity { get; set; }

        public SimpleProductViewModel Product { get; set; }
    }
}

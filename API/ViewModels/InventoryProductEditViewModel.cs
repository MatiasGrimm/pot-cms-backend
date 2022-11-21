using PotShop.API.Models.Entities;
using System;

namespace PotShop.API.ViewModels
{
    public class InventoryProductEditViewModel
    {
        public int Quantity { get; set; }

        public Guid ProductId { get; set; }
    }
}

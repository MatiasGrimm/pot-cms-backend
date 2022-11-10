using System;
using System.Collections.Generic;

namespace PotShop.API.Models.Entities
{
    public class Inventory
    {
        public Guid Id { get; set; }

        public Guid? locationId { get; set; }

        public Location Location { get; set; }

        public List<InventoryProduct> Products { get; set; }
    }
}

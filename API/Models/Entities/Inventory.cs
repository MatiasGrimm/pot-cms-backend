using System;

namespace PotShop.API.Models.Entities
{
    public class Inventory
    {
        public Guid Id { get; set; }


        public Location Location { get; set; }
    }
}

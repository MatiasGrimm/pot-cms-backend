using System;

namespace PotShop.API.Models.Entities
{
    public class Location : IDisabledEntity
    {
        public Guid id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int LeaderId { get; set; }

        public int Type { get; set; }

        public bool IsDisabled { get; set; }


        public Inventory Inventory { get; set; }
    }
}

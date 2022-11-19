using System;

namespace PotShop.API.Models.Entities
{
    public class Manager : ApiUser
    {
        public Guid LocationId { get; set; }

        public Location Location { get; set; }
    }
}

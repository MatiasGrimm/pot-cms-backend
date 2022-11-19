using System.Collections.Generic;

namespace PotShop.API.Models.Entities
{
    public class Staff : ApiUser
    {
        public Staff Manager { get; set; }

        public Location Location { get; set; }

        public StaffAccess StaffAccess { get; set; }

        public List<SalesHistory> SalesHistory { get; set; }
    }
}

using System;

namespace PotShop.API.ViewModels
{
    public class SimpleLocationViewModel
    {
        public Guid ManagerId { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.ViewModels
{
    public class CompanyViewModel
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string CVR { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public string Country { get; set; }

        public int UsersCount { get; set; }
    }
}

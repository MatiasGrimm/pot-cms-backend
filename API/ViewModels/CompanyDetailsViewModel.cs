using System.Collections.Generic;

namespace PotShop.API.ViewModels
{
    public class CompanyDetailsViewModel : CompanyViewModel
    {
        public List<UserViewModel> Users { get; set; }
    }
}

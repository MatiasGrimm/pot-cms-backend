using System.Collections.Generic;

namespace PotShop.API.ViewModels
{
    public class CompanyDetailsViewModel : CompanyViewModel
    {
        public List<StaffViewModel> Users { get; set; }
    }
}

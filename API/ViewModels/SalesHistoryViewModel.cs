using PotShop.API.Models.Entities;
using System;

namespace PotShop.API.ViewModels
{
    public class SalesHistoryViewModel : SimpleSalesHistoryViewModel
    {

        public ProductList ProductList { get; set; }

        public ApiUser Employee { get; set; }

        public Location Shop { get; set; }
    }
}

using PotShop.API.Models.Entities;
using System;
using System.Collections.Generic;

namespace PotShop.API.ViewModels
{
    public class SalesHistoryViewModel : SimpleSalesHistoryViewModel
    {
        public List<Product> Products { get; set; }

        public Staff Staff { get; set; }

        public Location Shop { get; set; }
    }
}

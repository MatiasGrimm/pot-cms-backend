using System;

namespace PotShop.API.ViewModels
{
    public class SimpleSalesHistoryViewModel
    {
        public Guid ShopId { get; set; }

        public string EmployeeId { get; set; }

        public string CustomerCPR { get; set; }

        public DateTime PurchaseTime { get; set; }

        public float TotalPrice { get; set; }
    }
}

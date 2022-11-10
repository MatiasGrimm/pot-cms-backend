using System;

namespace PotShop.API.Models.Entities
{
    public class SalesHistory
    {
        public Guid Id { get; set; }

        public ProductList ProductList { get; set; }

        public Guid ShopId { get; set; }

        public string EmployeeId { get; set; }

        public string CustomerCPR { get; set; }

        public DateTime PurchaseTime { get; set; }

        public float TotalPrice { get; set; }


        public ApiUser Employee { get; set; }

        public Location Shop { get; set; }
    }
}

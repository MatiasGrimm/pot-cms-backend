using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotShop.API.Models.Entities
{
    public class SalesHistory
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public List<Product> Products { get; set; }

        public Guid ShopId { get; set; }

        public string EmployeeId { get; set; }

        public string CustomerCPR { get; set; }

        public DateTime PurchaseTime { get; set; }

        public float TotalPrice { get; set; }


        public ApiUser Employee { get; set; }

        public Location Shop { get; set; }
    }
}

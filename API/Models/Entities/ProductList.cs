using System;
using System.Collections.Generic;

namespace PotShop.API.Models.Entities
{
    public class ProductList
    {
        public Guid Id { get; set; }

        public List<Product> Products { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace PotShop.API.Models.Entities
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}

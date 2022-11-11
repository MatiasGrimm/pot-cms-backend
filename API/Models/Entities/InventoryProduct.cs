﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotShop.API.Models.Entities
{
    public class InventoryProduct
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid InventoryId { get; set; }

        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}

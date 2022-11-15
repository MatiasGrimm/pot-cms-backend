using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotShop.API.Models.Entities
{
    public class Location : IDisabledEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public int ManagerId { get; set; }

        public int Type { get; set; }

        public bool IsDisabled { get; set; }

        public List<StaffAccess> StaffAccess { get; set; }

        public Inventory Inventory { get; set; }
    }
}

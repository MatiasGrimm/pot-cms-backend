using PotShop.API.Models.Entities.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PotShop.API.Models.Entities
{
    public class StaffAccess
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string StaffId { get; set; }

        public Access Access { get; set; }

        public Staff Staff { get; set; }

        public Location Location { get; set; }
    }
}

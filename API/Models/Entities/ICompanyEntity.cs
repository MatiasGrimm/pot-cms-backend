using System;

namespace PotShop.API.Models.Entities
{
    public interface ICompanyEntity
    {
        public Guid CompanyId { get; set; }

        public Company Company { get; set; }
    }
}

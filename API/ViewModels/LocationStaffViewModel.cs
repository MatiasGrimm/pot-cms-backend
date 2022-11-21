using System;

namespace PotShop.API.ViewModels
{
    /// <summary>
    /// ViewModel for changing staff id directly on a location with only the id's
    /// </summary>
    public class LocationStaffViewModel
    {
        public Guid LocationId { get; set; }
        public string ManagerId { get; set; }
    }
}

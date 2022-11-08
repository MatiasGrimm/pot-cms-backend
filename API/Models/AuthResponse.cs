using PotShop.API.Models.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace PotShop.API.Models
{
    public class AuthResponse
    {
        public ApiUser User { get; set; }
        public IList<string> Roles { get; set; }
        public ClaimsIdentity Identity { get; set; }
        public string RefreshTokenId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.ViewModels
{
    /// <summary>
    /// Response sent to client when successfully authenticated
    /// </summary>
    public class AuthResponseViewModel
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public DateTimeOffset Expires { get; set; }
        public string RefreshToken { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public List<string> Roles { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PotShop.API.Models
{
    public class SiteOptions
    {
        public string FrontEndUrl { get; set; }

        public string[] AdminEmailAdresses { get; set; }

        public string ContactEmailAddress { get; set; }
    }
}

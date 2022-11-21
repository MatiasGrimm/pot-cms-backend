﻿using PotShop.API.Models.Entities;
using System;
using System.Collections.Generic;

namespace PotShop.API.ViewModels
{
    public class StaffViewModel
    {
        public string Id { get; set; }

        public DateTimeOffset Created { get; set; }

        public DateTimeOffset Updated { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public bool IsDisabled { get; set; }

        public SimpleLocationViewModel Location { get; set; }

        public List<string> Roles { get; set; }
    }
}
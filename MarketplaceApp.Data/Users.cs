﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketplaceApp.Data
{
    public class Users
    {
        public string Name { get; set; }
        public string Email { get; set; }

        public Users(string name, string email)
        {
            Name = name;
            Email = email;
        }
    }
}

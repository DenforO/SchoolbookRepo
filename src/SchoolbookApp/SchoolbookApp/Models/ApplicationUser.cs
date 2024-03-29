﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int? SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public override string ToString()
        {
            return Name + " " + Surname;
        }
    }
}

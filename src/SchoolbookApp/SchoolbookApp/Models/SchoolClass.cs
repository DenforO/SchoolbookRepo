using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Models
{
    public class SchoolClass
    {
        public int Id { get; set; }

        public int Num { get; set; }

        public char Letter { get; set; }

        public ICollection<Subject> Subjects { get; set; }
        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public SchoolClass()
        {

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Models
{
    public class Grade : Review
    {
        public int Value { get; set; }
        public string Basis { get; set; }
        public bool IsSemesterGrade { get; set; }
        public bool IsFinalGrade { get; set; }

        public Grade()
        {

        }

    }
}

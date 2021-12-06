using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public SchoolClass SchoolClass { get; set; }
        public DateTime DateTime { get; set; }

        public Review()
        {

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Models
{
    public class Absence : Review
    {
        public bool Half { get; set; }
        public bool isExcused { get; set; }

    }
}

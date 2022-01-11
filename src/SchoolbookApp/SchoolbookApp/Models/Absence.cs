using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Models
{
    public class Absence : Review
    {
        [Display(Name = "Половин отсъствие")]
        public bool Half { get; set; }

        [Display(Name = "Извинено")]
        public bool isExcused { get; set; }

    }
}

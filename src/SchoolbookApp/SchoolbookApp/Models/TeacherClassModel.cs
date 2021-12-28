using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Models
{
    public class TeacherClassModel
    {
        public Grade Grade { get; set; }
        public Absence Absence { get; set; }
        public Note Note { get; set; }
        public int ClassId { get; set; }

        public TeacherClassModel()
        {

        }
    }
}

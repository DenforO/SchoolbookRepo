using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan Time { get; set; }
        public int TypeId { get; set; }
        public string TeacherId { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public Subject()
        {

        }

    }
}

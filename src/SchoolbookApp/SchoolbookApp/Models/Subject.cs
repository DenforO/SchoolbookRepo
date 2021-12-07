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

        public int? SubjectTypeId { get; set; }
        public SubjectType SubjectType { get; set; }

        public string TeacherId { get; set; }
        public SchoolClass SchoolClass { get; set; }


        public ICollection<Review> Reviews { get; set; }

        public Subject()
        {

        }

    }
}

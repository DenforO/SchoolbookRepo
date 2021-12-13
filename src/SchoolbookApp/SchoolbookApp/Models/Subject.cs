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

        public int? SchoolClassId { get; set; }
        public SchoolClass SchoolClass { get; set; }

        public ICollection<Grade> Grades { get; set; }
        public ICollection<Note> Notes { get; set; }
        public ICollection<Absence> Absences { get; set; }

        public Subject()
        {

        }

    }
}

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

        public int? SubjectId { get; set; }
        public Subject Subject { get; set; } //Няма предварително генерирани полета във формите

        public DateTime DateTime { get; set; }

        public Review()
        {

        }

    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolbookApp.Data;
using SchoolbookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfilesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("TeacherMain", "Profiles");
        }

        public async Task<IActionResult> TeacherMain()
        {
            IdentityUser usr = await GetCurrentUserAsync();
                        
            var subjectsByTeacher = _context.Subject.Where(x => x.TeacherId == usr.Id).ToList();
            var normalizedName = _context.Users.Where(x => x.Id == usr.Id).Select(x => x.NormalizedUserName).FirstOrDefault(); // предстои да се взимат имената
            var isMainTeacher = true;
            var mainClass = _context.SchoolClass.Where(x => x.Id == 3) // хардкоудната стойност засега
                                                .Select(x => new
                                                {
                                                    Id = x.Id,
                                                    ClassName = x.Num + " " + x.Letter
                                                })
                                                .FirstOrDefault();
            var gradesList = _context.Grade.Select(x => x.Value).ToList();
            var avgScore = gradesList.Sum()/gradesList.Count; //Да се вземе средната оценка за даден клас

            ViewBag.SubjectTypes = _context.Subject.Where(x => x.TeacherId == usr.Id)
                                                    .Select(x => x.SubjectType)      
                                                    .Distinct()                      
                                                    .ToList();
                       
            ViewBag.SchoolClasses = _context.SchoolClass.ToList().Join(
                                        subjectsByTeacher,
                                        x => x.Id,
                                        y => y.SchoolClassId,
                                        (x, y) => (x, y)).ToList();

            ViewBag.Name = normalizedName;
            ViewBag.IsMainTeacher = isMainTeacher;
            ViewBag.MainClassId = mainClass.Id;
            ViewBag.MainClassName = mainClass.ClassName;
            ViewBag.AvgScore = avgScore;

            return View("TeacherMain");
        }


        public async Task<IActionResult> TeacherClass()
        {
            return View("TeacherClass");
        }
        public async Task<IActionResult> StudentMain()
        {
            ApplicationUser user = await GetCurrentUserAsync();

            var schoolClass = _context.SchoolClass.Where(w => w.Id == user.SchoolClassId).FirstOrDefault();
            var grades = _context.Grade.
                Where(w => w.StudentId == user.Id && w.IsSemesterGrade==false && w.IsFinalGrade==false)
                .Select(s => s.Value).ToList();

            double avgScore = 0;
            if(grades.Count>0)
                avgScore=(double)grades.Sum() / (double)grades.Count();

            var schoolSubjects = _context.Subject.Where(w => w.SchoolClassId == user.SchoolClassId).ToList();
            List<ApplicationUser> teachers = new List<ApplicationUser>();
            List<SubjectType> subjectTypes = new List<SubjectType>();
            foreach (var subject in schoolSubjects)
            { 
                var teachersTemp = _context.Users.Where(w => w.Id == subject.TeacherId).ToList();
                teachers.Add(teachersTemp[0]);
                var subjectTypesTemp = _context.SubjectType.Where(w => w.Id == subject.SubjectTypeId).ToList();
                subjectTypes.Add(subjectTypesTemp[0]); 
            }
            
            ViewBag.Name = user.Name;
            ViewBag.Surname = user.Surname;
            ViewBag.StudentClassNum = schoolClass.Num;
            ViewBag.StudentClassLetter = schoolClass.Letter;
            ViewBag.AverageScore = Math.Round(avgScore,2);
            ViewBag.RankInClass = GetStudentRankInClass(user, avgScore);
            ViewBag.AverageClassScore = GetClassAvgScore(user.SchoolClassId);
            ViewBag.SubjectTypes = subjectTypes;
            ViewBag.Subjects = schoolSubjects;
            ViewBag.Teachers = teachers;
            return View("StudentMain");
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        private int GetStudentRankInClass(ApplicationUser user, double avgScore)
        {
            var students = _context.Users.Where(w=>w.SchoolClassId == user.SchoolClassId).ToList();
            int positionInClass = students.Count();
            int counter = -1;
            foreach (var student in students)
            {
               var studentGrades = _context.Grade
                   .Where(w => w.StudentId == student.Id && w.IsSemesterGrade == false && w.IsFinalGrade == false)
                   .Select(s => s.Value).ToList();
               var avgStudentScore = studentGrades.Sum() / studentGrades.Count();

               if (avgScore > avgStudentScore)
                    counter++; 
            }

            return positionInClass - counter;  
        }
        private double GetClassAvgScore(int studentClassId)
        {
            var students = _context.Users.Where(w => w.SchoolClassId == studentClassId).Select(s => s).ToList();
            double avgScore = 0;
            foreach (var student in students)
            {
                var studentGrades = _context.Grade
                   .Where(w => w.StudentId == student.Id && w.IsSemesterGrade == false && w.IsFinalGrade == false)
                   .Select(s => s.Value).ToList();
                double avgStudentScore = (double)studentGrades.Sum() / studentGrades.Count();
                avgScore += avgStudentScore;
            }
            return Math.Round(avgScore/students.Count(),2);
        }

    }
}

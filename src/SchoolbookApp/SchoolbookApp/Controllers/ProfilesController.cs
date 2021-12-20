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

        public async Task<IActionResult> TeacherMain()
        {
            IdentityUser usr = await GetCurrentUserAsync();

            var subjectsByTeacher = _context.Subject.Where(x => x.TeacherId == usr.Id).ToList();
            var fullName = _context.Users.Where(x => x.Id == usr.Id).Select(x => x.Name + " " + x.Surname).FirstOrDefault();
            var isMainTeacher = true;
            int mainClassID = _context.UserSchoolClass.Where(x => x.UserId == usr.Id)
                                                .Select(x => x.SchoolClassId)
                                                .FirstOrDefault();
            var getMainClass = _context.SchoolClass.Where(x => x.Id == mainClassID).FirstOrDefault();
            var mainClassName = getMainClass.Num + " " + getMainClass.Letter;
            var gradesList = _context.Grade.Select(x => x.Value).ToList();
            var avgScore = gradesList.Sum() / gradesList.Count;

            ViewBag.SubjectTypes = _context.Subject.Where(x => x.TeacherId == usr.Id)
                                                    .Select(x => x.SubjectType)
                                                    .Distinct()
                                                    .ToList();

            ViewBag.SchoolClasses = _context.SchoolClass.ToList().Join(
                                        subjectsByTeacher,
                                        x => x.Id,
                                        y => y.SchoolClassId,
                                        (x, y) => (x, y)).ToList();

            ViewBag.Name = fullName;
            ViewBag.IsMainTeacher = isMainTeacher;
            ViewBag.MainClassId = mainClassID;
            ViewBag.MainClassName = mainClassName;
            ViewBag.AvgScore = avgScore;

            return View("TeacherMain");
        }

        public async Task<IActionResult> TeacherClass(int id)
        {
            var isMainTeacher = true;
            var usersClassId = _context.UserSchoolClass.Where(x => x.SchoolClassId == id).Select(x => x.UserId).ToList();
            var usersClassName = _context.SchoolClass.Where(x => x.Id == id).Select(x => x.Num + " " + x.Letter).FirstOrDefault();
            var studentsRoleUsers = _context.UserRoles.Where(y => y.RoleId == "abd35139-96cd-4798-972c-20e981166630").Select(x => x.UserId).ToList();
            List<string> studentsOnly = new List<string>();
            foreach (var user in usersClassId)
            {
                foreach (var student in studentsRoleUsers)
                {
                    if (user == student)
                        studentsOnly.Add(student);
                }
            }

            var gradeStudentTable1 = _context.Grade.ToList()
                                                   .Join(_context.Users,
                                                          x => x.StudentId,
                                                          y => y.Id,
                                                          (x, y) => new { x, y }).ToList();

            var gradeStudentTable = _context.Grade.ToList()
                                                  .Join(_context.Users,
                                                         x => x.StudentId,
                                                         y => y.Id,
                                                         (x, y) => new { x, y }).ToList()
                                                  .Where(x => studentsOnly.Any(y => y == x.x.StudentId)).ToList();

            var studentObj = gradeStudentTable.GroupBy(x => x.x.StudentId)
                                              .Select(x => new
                                              {
                                                  Id = x.Key,
                                                  FullName = x.Select(y => y.y.Name + " " + y.y.Surname).FirstOrDefault(),
                                                  Grades = string.Join(", ", x.Select(x => x.x.Value).ToList().Select(x => x.ToString()))
                                              }
                                              ).ToList();            

            ViewBag.IsMainTeacher = isMainTeacher;
            ViewBag.ClassName = usersClassName;
            ViewBag.StudentObj = studentObj;
            return View();
        }

        public void SaveChanges (int grade,string reason, string note, string noteData, string absence, string absenceData)
        {

        }
        public async Task<IActionResult> ParentMain()
        {
            return View("ParentMain");
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolbookApp.Data;
using SchoolbookApp.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
            ApplicationUser usr = await GetCurrentUserAsync();
            var subjectsByTeacher = _context.Subject.Where(x => x.TeacherId == usr.Id).ToList();
            var fullName = _context.Users.Where(x => x.Id == usr.Id).Select(x => x.Name + " " + x.Surname).FirstOrDefault();

            var isMainTeacher = true;
            int mainClassID = usr.SchoolClassId.HasValue ? usr.SchoolClassId.Value : 0;
            if (mainClassID == null || mainClassID == 0)
                isMainTeacher = false;

            if (isMainTeacher)
            {
                var getMainClass = _context.SchoolClass.Where(x => x.Id == mainClassID).FirstOrDefault();
                string mainClassName = getMainClass.Num + " " + getMainClass.Letter;
                var studentsInMainClass = _context.Users.Where(y => y.SchoolClassId == mainClassID).Select(y => y.Id).ToList();
                List<int> gradesList = new List<int>();

                foreach (var grade in _context.Grade)
                {
                    foreach (var studentId in studentsInMainClass)
                    {
                        var gradeValue = (grade.StudentId == studentId && grade.IsSemesterGrade == false && grade.IsFinalGrade == false) ? grade.Value : 0;
                        if (gradeValue != 0)
                            gradesList.Add(gradeValue);
                    }
                }

                double avgScore = 2;
                if (gradesList.Count > 0)
                    avgScore = (double)gradesList.Sum() / (double)gradesList.Count();
                ViewBag.MainClassName = mainClassName;
                ViewBag.AvgScore = Math.Round(avgScore, 2);
            }

            ViewBag.SubjectTypes = _context.Subject.Where(x => x.TeacherId == usr.Id)
                                                    .Select(x => x.SubjectType)
                                                    .Distinct()
                                                    .ToList();

            ViewBag.SchoolClasses = _context.SchoolClass.ToList().Join(subjectsByTeacher,
                                                                       x => x.Id,
                                                                       y => y.SchoolClassId,
                                                                       (x, y) => (x, y)).ToList();

            ViewBag.Name = fullName;
            ViewBag.IsMainTeacher = isMainTeacher;
            ViewBag.MainClassId = mainClassID;

            return View("TeacherMain");
        }

        public async Task<IActionResult> TeacherClass(int id)
        {
            ApplicationUser usr = await GetCurrentUserAsync();

            var isMainTeacher = true;
            int mainClassID = usr.SchoolClassId.HasValue ? usr.SchoolClassId.Value : 0;
            if (mainClassID == null || mainClassID == 0 || mainClassID != id)
                isMainTeacher = false;
            var usersClassId = _context.Users.Where(x => x.SchoolClassId == id).Select(x => x.Id).ToList();
            var usersClassName = _context.SchoolClass.Where(x => x.Id == id).Select(x => x.Num + " " + x.Letter).FirstOrDefault();
            var studentRoleUsers = _userManager.GetUsersInRoleAsync("Student").Result.Select(x => x.Id).ToList();
            List<string> studentsOnly = new List<string>();
            foreach (var user in usersClassId)
            {
                foreach (var student in studentRoleUsers)
                {
                    if (user == student)
                        studentsOnly.Add(student);
                }
            }

            var usersTable = _context.Users.Where(x => studentsOnly.Any(y => y == x.Id)).ToList();
            var gradesTable = _context.Grade.Where(x => studentsOnly.Any(y => y == x.StudentId)).ToList();
            var studentUsersTable = usersTable.GroupBy(x => x.Id)
                                              .Select(x => new
                                              {
                                                  Id = x.Key,
                                                  FullName = x.Select(y => y.Name + " " + y.Surname).FirstOrDefault()
                                              }).ToList();
            var studentGradesTable = gradesTable.GroupBy(x => x.StudentId)
                                                .Select(x => new
                                                {
                                                    Id = x.Key,
                                                    Grades = string.Join(", ", x.Select(x => x.Value).ToList().Select(x => x.ToString()))
                                                }).ToList();
            var studentObj = studentUsersTable.Select(x => new
            {
                Id = x.Id,
                FullName = x.FullName,
                Grades = studentGradesTable.Where(y => y.Id == x.Id).DefaultIfEmpty().Select(y => y == null ? "" : y.Grades).FirstOrDefault()
            }).ToList();

            var absencestudentTable = _context.Absence.Join(_context.Users,
                                                             x => x.StudentId,
                                                             y => y.Id,
                                                             (x, y) => new { x, y })
                                                      .Where(x => studentsOnly.Any(y => y == x.y.Id))
                                                      .ToList();

            var studentAbs = absencestudentTable.GroupBy(x => x.x.StudentId)
                                                .Select(x => new
                                                {
                                                    Absence = x.Where(x => x.x.isExcused == false).Select(x => x.x.Half),
                                                    Date = x.Select(x => x.x.DateTime)
                                                }).ToList();

            ViewBag.SubjectId = _context.Subject.Where(x => x.TeacherId == usr.Id && x.SchoolClassId == id)
                                                .Select(x => x.Id)
                                                .FirstOrDefault();
            ViewBag.IsMainTeacher = isMainTeacher;
            ViewBag.ClassId = id;
            ViewBag.ClassName = usersClassName;
            ViewBag.StudentObj = studentObj.OrderBy(x => x.FullName);
            ViewBag.Absences = studentAbs;

            return View();
        }

        public async Task<IActionResult> TeacherProgram()
        {
            ApplicationUser user = await GetCurrentUserAsync();

            var classes = _context.Subject.Where(x => x.TeacherId == user.Id).ToList();
            foreach (var el in classes)
            {
                el.Room = _context.Room.Where(x => x.Id == el.RoomId).FirstOrDefault();
                el.SchoolClass = _context.SchoolClass.Where(x => x.Id == el.SchoolClassId).FirstOrDefault();
                el.SubjectType = _context.SubjectType.Where(x => x.Id == el.SubjectTypeId).FirstOrDefault();
            }
            var schoolClass = _context.SchoolClass.Where(w => w.Id == user.SchoolClassId).FirstOrDefault();

            ViewBag.TeacherName = user.Name + " " + user.Surname;
            return View(classes);
        }

        public IActionResult SaveChanges()
        {

            return View("ParentMain");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveChanges(TeacherClassModel model)
        {
            var classId = model.ClassId;
            if (ModelState.IsValid)
            {
                _context.Grade.AddRange(model.Grade);
                _context.Absence.AddRange(model.Absence);
                _context.Note.AddRange(model.Note);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("TeacherClass", new { id = classId });
        }

        public async Task<IActionResult> ParentMain()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var childrenId = _context.UserUser.Where(w => w.UserId == user.Id).Select(s => s.StudentId).ToList();
            List<ApplicationUser> children = new List<ApplicationUser>();
            Dictionary<string, double> childAvgGrades = new Dictionary<string, double>();
            foreach (var id in childrenId)
            {
                var child = _context.Users.Where(w => w.Id == id).FirstOrDefault();
                children.Add(child);
                var grades = _context.Grade.Where(w => w.StudentId == child.Id && w.IsSemesterGrade == false && w.IsFinalGrade == false).Select(s => s.Value).ToList();

                double avgScore = 0;
                if (grades.Count > 0)
                    avgScore = Math.Round((double)grades.Sum() / (double)grades.Count(), 2);

                childAvgGrades.Add(id, avgScore);
            }
            var teachers = await _userManager.GetUsersInRoleAsync("Teacher");

            List<SchoolClass> childrenClass = new List<SchoolClass>();
            List<ApplicationUser> headTeachers = new List<ApplicationUser>();

            foreach (var child in children)
            {
                var childClass = _context.SchoolClass.Where(w => w.Id == child.SchoolClassId).FirstOrDefault();
                childrenClass.Add(childClass);
                var headTeacher = teachers.Where(w => w.SchoolClassId == child.SchoolClassId).FirstOrDefault();
                headTeachers.Add(headTeacher);
            }

            ViewBag.ParentName = user.Name + " " + user.Surname;
            ViewBag.Children = children;
            ViewBag.ChildrenClass = childrenClass.Distinct();
            ViewBag.HeadTeachers = headTeachers.Distinct();
            ViewBag.ChildrenGrades = childAvgGrades;

            return View("ParentMain");
        }


        public async Task<IActionResult> StudentMain()
        {
            ApplicationUser user = await GetCurrentUserAsync();

            var schoolClass = _context.SchoolClass.Where(w => w.Id == user.SchoolClassId).FirstOrDefault();
            var grades = _context.Grade.
                Where(w => w.StudentId == user.Id && w.IsSemesterGrade == false && w.IsFinalGrade == false)
                .Select(s => s.Value).ToList();
            double avgScore = 0;
            if (grades.Count > 0)
                avgScore = (double)grades.Sum() / (double)grades.Count();
            else
                avgScore = 0;

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
            ViewBag.AverageScore = Math.Round(avgScore, 2);
            ViewBag.SubjectTypes = subjectTypes;
            ViewBag.Subjects = schoolSubjects;
            ViewBag.Teachers = teachers.Distinct();
            ViewBag.RankInClass = await GetStudentRankInClassAsync(user, avgScore);
            ViewBag.AverageClassScore = await GetClassAvgScoreAsync(user.SchoolClassId);

            return View("StudentMain");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminMain()
        {
            return View("AdminMain");
        }
        public async Task<IActionResult> ShowSearchResult(string searchType, char searchSchoolClassLetter, int searchSchoolClassNum, string searchProfileName, string searchProfileEmail)
        {
            if (searchType == "Subject")
            {
                return RedirectToRoute(new { action = "Index", controller = "Subjects", letter = searchSchoolClassLetter, num = searchSchoolClassNum });
            }
            else if (searchType == "SchoolClass")
            {
                return RedirectToRoute(new { action = "Index", controller = "SchoolClasses", letter = searchSchoolClassLetter, num = searchSchoolClassNum });
            }
            else
            {
                return RedirectToRoute(new { action = "Index", controller = "Users", name = searchProfileName, email = searchProfileEmail });
            }

        }


        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
        private async Task<int> GetStudentRankInClassAsync(ApplicationUser user, double avgScore)
        {
            var allStudents = await _userManager.GetUsersInRoleAsync("Student");
            var students = allStudents.Where(w => w.SchoolClassId == user.SchoolClassId).ToList();
            int positionInClass = students.Count();
            int counter = 0;
            foreach (var student in students)
            {
                var studentGrades = _context.Grade
                    .Where(w => w.StudentId == student.Id && w.IsSemesterGrade == false && w.IsFinalGrade == false)
                    .Select(s => s.Value).ToList();
                var avgStudentScore = 0;
                if (studentGrades.Count > 0)
                {
                    avgStudentScore = studentGrades.Sum() / studentGrades.Count();
                }


                if (avgScore > avgStudentScore && avgStudentScore > 0)
                    counter++;
            }

            return positionInClass - counter;
        }
        private async Task<double> GetClassAvgScoreAsync(int? studentClassId)
        {
            var allStudents = await _userManager.GetUsersInRoleAsync("Student");
            var students = allStudents.Where(w => w.SchoolClassId == studentClassId).ToList();
            double avgScore = 0;
            foreach (var student in students)
            {
                var studentGrades = _context.Grade
                   .Where(w => w.StudentId == student.Id && w.IsSemesterGrade == false && w.IsFinalGrade == false)
                   .Select(s => s.Value).ToList();
                double avgStudentScore = 0;
                if (studentGrades.Count > 0)
                    avgStudentScore = (double)studentGrades.Sum() / studentGrades.Count();

                avgScore += avgStudentScore;
            }
            return Math.Round(avgScore / students.Count(), 2);
        }

    }
}

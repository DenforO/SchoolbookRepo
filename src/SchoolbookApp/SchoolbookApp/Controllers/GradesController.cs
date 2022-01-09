using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolbookApp.Data;
using SchoolbookApp.Models;

namespace SchoolbookApp.Controllers
{
    public class GradesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GradesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Grades
        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            
            //return View(await _context.Grade.Where(x => x.StudentId == user.Id).ToListAsync());
            return View();
        }
        //GET: Взима оценките по ID на предмет
        public async Task<IActionResult> GetStudentGrades(int id)
        {
            ApplicationUser user = await GetCurrentUserAsync();

            var schoolClass= _context.SchoolClass.Where(w => w.Id == user.SchoolClassId).FirstOrDefault();
            var teacherId = _context.Subject.Where(w => w.SubjectTypeId == id && w.SchoolClassId == user.SchoolClassId).Select(s => s.TeacherId).FirstOrDefault();
            var grades = _context.Grade.Where(w => w.Subject.SubjectType.Id == id && w.StudentId==user.Id && w.IsSemesterGrade==false && w.IsFinalGrade==false).ToList();
            var gradesValue = grades.Select(s => s.Value).ToList();

            ViewBag.FirstName = user.Name;
            ViewBag.LastName = user.Surname;
            ViewBag.StudentClassNum = schoolClass.Num;
            ViewBag.StudentClassLetter = schoolClass.Letter;
            ViewBag.StudentGrades = grades;
            ViewBag.AverageScore = Math.Round((double)gradesValue.Sum() / (double)gradesValue.Count(),2); 
            ViewBag.SubjectName = _context.SubjectType.Where(w => w.Id == id).Select(s => s.Name).FirstOrDefault();
            ViewBag.FirstSemesterGrade = _context.Grade.Where(w => w.Subject.SubjectTypeId == id && w.StudentId == user.Id && w.Basis == "Оценка за I-Ви срок").FirstOrDefault();
            ViewBag.SecondSemesterGrade = _context.Grade.Where(w => w.Subject.SubjectTypeId == id && w.StudentId == user.Id && w.Basis == "Оценка за II-ри срок").FirstOrDefault();
            ViewBag.FinalGrade = _context.Grade.Where(w => w.Subject.SubjectTypeId == id && w.StudentId == user.Id && w.IsFinalGrade == true).FirstOrDefault();
            ViewBag.TeacherFirstName= _context.Users.Where(w => w.Id == teacherId).Select(s => s.Name).FirstOrDefault();
            ViewBag.TeacherLastName= _context.Users.Where(w => w.Id == teacherId).Select(s => s.Surname).FirstOrDefault();

            return View("Index");
        }

        public async Task<IActionResult> GetAllGrades(string id)
        {
            var student = _context.Users.Where(w => w.Id == id).FirstOrDefault();
            var grades = _context.Grade.Where(w => w.StudentId == id).ToList();
            var schoolClass = _context.SchoolClass.Where(w => w.Id == student.SchoolClassId).FirstOrDefault();
            var subjects = _context.Subject.ToList();
            Dictionary<int, string> subjectNames = new Dictionary<int, string>();
            foreach (var subject in subjects)
            {
                subject.SubjectType = _context.SubjectType.Where(w => w.Id == subject.SubjectTypeId).FirstOrDefault();
                subjectNames.Add(subject.Id, subject.SubjectType.Name);
            }

            
            ViewBag.StudentName = student.Name + " " + student.Surname;
            ViewBag.SchoolClassNum = schoolClass.Num;
            ViewBag.SchoolClassLetter = schoolClass.Letter;
            ViewBag.SubjectNames = subjectNames;
            
            return View("AllGrades",grades);
        }

        // GET: Grades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _context.Grade
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // Get: Grades/GradesTeacherClass
        public async Task<IActionResult> GradesTeacherClass(string? studentId, int? subjectId)
        {
            ViewBag.Student = _userManager.FindByIdAsync(studentId).Result;
            ViewBag.StudentId = studentId;
            ViewBag.SubjectId = subjectId;
            var grades = _context.Grade.Where(x => x.StudentId == studentId && x.SubjectId == subjectId);
            return View(grades);
        }

        // GET: Grades/Create
        public IActionResult Create(string? studentId, int? subjectId)
        {
            ViewBag.Student = _userManager.FindByIdAsync(studentId).Result;
            ViewBag.StudentId = studentId;
            ViewBag.SubjectId = subjectId;
            ViewBag.SubjectType = _context.SubjectType.Find(_context.Subject.Find(subjectId).SubjectTypeId).Name;
            return View();
        }

        // POST: Grades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Value,Basis,IsSemesterGrade,IsFinalGrade,Id,DateTime")] Grade grade, string? studentId, int? subjectId)
        {
            grade.StudentId = studentId;
            grade.SubjectId = subjectId;

            if (ModelState.IsValid)
            {
                _context.Grade.Add(grade);

                await _context.SaveChangesAsync();
            }
            return RedirectToRoute(new { action = "TeacherMain", controller = "Profiles" });
        }

        // GET: Grades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _context.Grade.FindAsync(id);
            if (grade == null)
            {
                return NotFound();
            }
            return View(grade);
        }

        // POST: Grades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Value,Basis,MyProperty,IsSemesterGrade,IsFinalGrade,Id,StudentId,DateTime")] Grade grade)
        {
            if (id != grade.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(grade);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GradeExists(grade.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(grade);
        }

        // GET: Grades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var grade = await _context.Grade
                .FirstOrDefaultAsync(m => m.Id == id);
            if (grade == null)
            {
                return NotFound();
            }

            return View(grade);
        }

        // POST: Grades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _context.Grade.FindAsync(id);
            _context.Grade.Remove(grade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GradeExists(int id)
        {
            return _context.Grade.Any(e => e.Id == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
       
    }
}

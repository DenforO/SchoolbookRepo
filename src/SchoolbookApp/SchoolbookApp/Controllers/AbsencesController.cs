using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolbookApp.Data;
using SchoolbookApp.Models;

namespace SchoolbookApp.Controllers
{
    public class AbsencesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AbsencesController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Absences
        public async Task<IActionResult> Index()
        {
            return View(await _context.Absence.ToListAsync());
        }
        public async Task<IActionResult> GetAbsencesAsStudent()
        {
            ApplicationUser user = await GetCurrentUserAsync();
            var absences = _context.Absence.Where(w => w.StudentId == user.Id).ToList();
            foreach (var absence in absences)
            {
               absence.Subject = _context.Subject.Where(w => w.Id == absence.SubjectId).FirstOrDefault();
               absence.Subject.SubjectType = _context.SubjectType.Where(w => w.Id == absence.Subject.SubjectTypeId).FirstOrDefault();
            }
            var subjects = _context.Absence.Where(w => w.StudentId == user.Id).Select(s => s.Subject).ToList();
            Dictionary<int, string> subjectNames = new Dictionary<int, string>();
            for(int i = 0;i<subjects.Count;i++)
            {
                subjects[i].SubjectType = _context.SubjectType.Where(w => w.Id == subjects[i].SubjectTypeId).FirstOrDefault();
                subjectNames.Add(i, subjects[i].SubjectType.Name);
            }

            ViewBag.SubjectNames = subjectNames;
            return View("Index", absences);
        }
        public async Task<IActionResult> GetAbsencesAsParent(string id)
        {
            var absences = _context.Absence.Where(w => w.StudentId == id).ToList();
            foreach (var absence in absences)
            {
                absence.Subject = _context.Subject.Where(w => w.Id == absence.SubjectId).FirstOrDefault();
                absence.Subject.SubjectType = _context.SubjectType.Where(w => w.Id == absence.Subject.SubjectTypeId).FirstOrDefault();
            }
            var subjects = _context.Absence.Where(w => w.StudentId == id).Select(s => s.Subject).ToList();
            Dictionary<int, string> subjectNames = new Dictionary<int, string>();
            for (int i = 0; i < subjects.Count; i++)
            {
                subjects[i].SubjectType = _context.SubjectType.Where(w => w.Id == subjects[i].SubjectTypeId).FirstOrDefault();
                subjectNames.Add(i, subjects[i].SubjectType.Name);
            }

            ViewBag.SubjectNames = subjectNames;
            return View("Index",absences);
        }

        // GET: Absences/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absence = await _context.Absence
                .FirstOrDefaultAsync(m => m.Id == id);
            if (absence == null)
            {
                return NotFound();
            }

            return View(absence);
        }

        // Get: Grades/GradesTeacherClass
        public async Task<IActionResult> AbsencesTeacherClass(string? studentId, int? subjectId)
        {
            ViewBag.Student = _userManager.FindByIdAsync(studentId).Result;
            ViewBag.StudentId = studentId;
            ViewBag.SubjectId = subjectId;
            var absences = _context.Absence.Where(x => x.StudentId == studentId && x.SubjectId == subjectId);
            return View(absences);
        }

        // GET: Absences/Create
        public IActionResult Create(string? studentId, int? subjectId)
        {
            ViewBag.Student = _userManager.FindByIdAsync(studentId).Result;
            ViewBag.StudentId = studentId;
            ViewBag.SubjectId = subjectId;
            var subject = _context.Subject.Find(subjectId);
            if(null != subject)
            {
                var subjectType = _context.SubjectType.Find(subject.SubjectTypeId);
                if (null != subjectType)
                {
                    ViewBag.SubjectType = subjectType.Name;
                }
            }
            return View();
        }

        // POST: Absences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Half,isExcused,Id,DateTime")] Absence absence, string? studentId, int? subjectId)
        {
            absence.StudentId = studentId;
            absence.SubjectId = subjectId;

            if (ModelState.IsValid)
            {
                _context.Absence.Add(absence);

                await _context.SaveChangesAsync();
            }
            return RedirectToRoute(new { action = "TeacherMain", controller = "Profiles" });
        }

        // GET: Absences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absence = await _context.Absence.FindAsync(id);
            ViewBag.Absence = absence;
            ViewBag.Student = await _userManager.FindByIdAsync(absence.StudentId);
            if (absence == null)
            {
                return NotFound();
            }
            return View(absence);
        }

        // POST: Absences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Half,isExcused,Id,StudentId,DateTime")] Absence absence)
        {
            if (id != absence.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(absence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AbsenceExists(absence.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToRoute(new { action = "Index", controller = "Home" });
            }
            return View(absence);
        }

        // GET: Absences/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absence = await _context.Absence
                .FirstOrDefaultAsync(m => m.Id == id);
            if (absence == null)
            {
                return NotFound();
            }

            return View(absence);
        }

        // POST: Absences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var absence = await _context.Absence.FindAsync(id);
            _context.Absence.Remove(absence);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AbsenceExists(int id)
        {
            return _context.Absence.Any(e => e.Id == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}

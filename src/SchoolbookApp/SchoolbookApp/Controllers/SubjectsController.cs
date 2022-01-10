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
    public class SubjectsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SubjectsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> StudentsProgram()
        {
            ApplicationUser user = await GetCurrentUserAsync();

            var subjects = _context.Subject.Where(w => w.SchoolClassId == user.SchoolClassId).ToList();
            foreach (var subject in subjects)
            {
                subject.SubjectType = _context.SubjectType.Where(w => w.Id == subject.SubjectTypeId).FirstOrDefault();
            }
            var schoolClass = _context.SchoolClass.Where(w => w.Id == user.SchoolClassId).FirstOrDefault();

            ViewBag.ClassNum = schoolClass.Num;
            ViewBag.ClassLetter = schoolClass.Letter;
            return View(subjects);
        }

        // GET: Subjects
        public async Task<IActionResult> Index(char? letter, int? num)
        {
            if (letter == 0)
            {
                letter = null;
            }
            if (num == 0)
            {
                num = null;
            }

            ViewBag.SchoolClasses = _context.SchoolClass.ToList();
            ViewBag.Teachers = _userManager.Users.ToList();
            ViewBag.SubjectTypes = _context.SubjectType.ToList();
            if (letter == null && num == null) 
            {
                return View(await _context.Subject
                                                .OrderBy(x => x.SchoolClass.Num)
                                                .ThenBy(x => x.SchoolClass.Letter)
                                                .ThenBy(x => x.SubjectType)
                                                .ToListAsync());
            }
            else if (letter != null && num != null)
            {
                return View(await _context.Subject
                                                .Where(x => x.SchoolClass.Letter == letter)
                                                .Where(x => x.SchoolClass.Num == num)
                                                .OrderBy(x => x.SchoolClass.Num)
                                                .ThenBy(x => x.SchoolClass.Letter)
                                                .ThenBy(x => x.SubjectType)
                                                .ToListAsync());
            }
            else
            {
                return View(await _context.Subject
                                                .Where(x => (x.SchoolClass.Letter == letter) || (x.SchoolClass.Num == num))
                                                .OrderBy(x => x.SchoolClass.Num)
                                                .ThenBy(x => x.SchoolClass.Letter)
                                                .ThenBy(x => x.SubjectType)
                                                .ToListAsync());
            }
        }

        // GET: Subjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.SchoolClass = _context.SchoolClass.Where(x => x.Id == _context.Subject.Find(id).SchoolClassId).Single();
            ViewBag.Teacher = _userManager.Users.Where(x => x.Id == _context.Subject.Find(id).TeacherId).Single();
            ViewBag.SubjectType = _context.SubjectType.Where(x => x.Id == _context.Subject.Find(id).SubjectTypeId).Single();
            ViewBag.Room = _context.Room.Where(x=>x.Id== _context.Subject.Find(id).RoomId).Single();

            var subject = await _context.Subject
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // GET: Subjects/Create
        public IActionResult Create()
        {
            ViewBag.Teachers = _userManager.GetUsersInRoleAsync("Teacher").Result;
            ViewBag.SubjectTypes = _context.SubjectType.ToList();
            ViewBag.Rooms = _context.Room.ToList();
            ViewBag.Subjects = _context.Subject.ToListAsync();
            //List<string> subjectTypes = new List<string>();
            //foreach (var subjectType in _context.SubjectType)
            //{
            //    subjectTypes.Add(subjectType.Name);
            //}
            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Day,Time,SubjectTypeId,RoomId")] Subject subject, int schoolClassNum, char schoolClassLetter, string teacherEmail)
        {
            var subjects= _context.Subject.ToList();
            bool invalid = false;
            foreach (var item in subjects)
            {
                if (item.Time + TimeSpan.FromMinutes(40) > subject.Time 
                    && item.Time - TimeSpan.FromMinutes(40) < subject.Time
                    && item.Room == subject.Room
                    && item.Day == subject.Day)
                {
                    invalid = true;
                    break;
                }
            }
            if (ModelState.IsValid && !invalid)
            {
                subject.TeacherId = _userManager.FindByEmailAsync(teacherEmail).Result.Id;
                subject.SchoolClassId = _context.SchoolClass.ToList().Where(x => x.Letter == schoolClassLetter && x.Num == schoolClassNum).Single().Id;
                _context.Add(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError(string.Empty, "Избраната стая е заета в избрания ден и час");
            return Create();
        }

        // GET: Subjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.SchoolClass = _context.SchoolClass.Where(x => x.Id == _context.Subject.Find(id).SchoolClassId).Single();
            ViewBag.Teacher = _userManager.Users.Where(x => x.Id == _context.Subject.Find(id).TeacherId).Single();
            ViewBag.SubjectType = _context.SubjectType.Where(x => x.Id == _context.Subject.Find(id).SubjectTypeId).Single();

            var subject = await _context.Subject.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Subject subject)
        {
            if (id != subject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Subject.Find(id).Day = subject.Day;
                    _context.Subject.Find(id).Time = subject.Time;
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectExists(subject.Id))
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
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.SchoolClass = _context.SchoolClass.Where(x => x.Id == _context.Subject.Find(id).SchoolClassId).Single();
            ViewBag.Teacher = _userManager.Users.Where(x => x.Id == _context.Subject.Find(id).TeacherId).Single();
            ViewBag.SubjectType = _context.SubjectType.Where(x => x.Id == _context.Subject.Find(id).SubjectTypeId).Single();

            var subject = await _context.Subject
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subject == null)
            {
                return NotFound();
            }

            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subject = await _context.Subject.FindAsync(id);
            _context.Subject.Remove(subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectExists(int id)
        {
            return _context.Subject.Any(e => e.Id == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}

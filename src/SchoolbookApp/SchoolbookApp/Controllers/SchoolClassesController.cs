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
    public class SchoolClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SchoolClassesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SchoolClasses
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

            ViewBag.Teachers = _userManager
                                    .GetUsersInRoleAsync("Teacher")
                                    .Result;

            if (letter == null && num == null)
            {
                return View(_context.SchoolClass.ToListAsync().Result.OrderBy(x => x.Num).ThenBy(x => x.Letter));
            }
            else if (letter != null && num != null)
            {
                return View(await _context.SchoolClass
                                                    .Where(x => (letter != null && x.Letter == letter) && (num != null && x.Num == num))
                                                    .OrderBy(x => x.Num)
                                                    .ThenBy(x => x.Letter)
                                                    .ToListAsync());
            }
            else
            {
                return View(await _context.SchoolClass
                                                    .Where(x => (letter != null && x.Letter == letter) || (num != null && x.Num == num))
                                                    .OrderBy(x => x.Num)
                                                    .ThenBy(x => x.Letter)
                                                    .ToListAsync());
            }
        }

        // GET: SchoolClasses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Students = _userManager
                                        .GetUsersInRoleAsync("Student")
                                        .Result
                                        .Where(x => x.SchoolClassId == id)
                                        .OrderBy(x => x.Name)
                                        .ThenBy(x => x.Surname);

            ViewBag.Teacher = _userManager
                                        .GetUsersInRoleAsync("Teacher")
                                        .Result
                                        .Where(x => x.SchoolClassId == id)
                                        .SingleOrDefault();

            var schoolClass = await _context.SchoolClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schoolClass == null)
            {
                return NotFound();
            }

            return View(schoolClass);
        }

        // GET: SchoolClasses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SchoolClasses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Num,Letter")] SchoolClass schoolClass)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schoolClass);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schoolClass);
        }

        // GET: SchoolClasses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolClass = await _context.SchoolClass.FindAsync(id);
            if (schoolClass == null)
            {
                return NotFound();
            }
            return View(schoolClass);
        }

        // POST: SchoolClasses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Num,Letter")] SchoolClass schoolClass)
        {
            if (id != schoolClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schoolClass);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolClassExists(schoolClass.Id))
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
            return View(schoolClass);
        }

        // GET: SchoolClasses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolClass = await _context.SchoolClass
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schoolClass == null)
            {
                return NotFound();
            }

            return View(schoolClass);
        }

        // POST: SchoolClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schoolClass = await _context.SchoolClass.FindAsync(id);
            _context.SchoolClass.Remove(schoolClass);

            foreach (var subject in _context.Subject)
            {
                if (subject.SchoolClassId == id)
                {
                    _context.Subject.Remove(subject);
                }
            }
            foreach (var user in _userManager.Users)
            {
                if (user.SchoolClassId == id)
                {
                    user.SchoolClassId = null;
                    await _userManager.UpdateAsync(user);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchoolClassExists(int id)
        {
            return _context.SchoolClass.Any(e => e.Id == id);
        }
    }
}

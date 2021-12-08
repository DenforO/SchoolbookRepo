using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public AbsencesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Absences
        public async Task<IActionResult> Index()
        {
            return View(await _context.Absence.ToListAsync());
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

        // GET: Absences/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Absences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Half,isExcused,Id,StudentId,DateTime")] Absence absence)
        {
            if (ModelState.IsValid)
            {
                _context.Add(absence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(absence);
        }

        // GET: Absences/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var absence = await _context.Absence.FindAsync(id);
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
                return RedirectToAction(nameof(Index));
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
    }
}

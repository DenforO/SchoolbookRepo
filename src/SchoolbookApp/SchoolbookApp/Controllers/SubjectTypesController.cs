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
    public class SubjectTypesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubjectTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubjectTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.SubjectType.ToListAsync());
        }

        // GET: SubjectTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectType = await _context.SubjectType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subjectType == null)
            {
                return NotFound();
            }

            return View(subjectType);
        }

        // GET: SubjectTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SubjectTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] SubjectType subjectType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subjectType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subjectType);
        }

        // GET: SubjectTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectType = await _context.SubjectType.FindAsync(id);
            if (subjectType == null)
            {
                return NotFound();
            }
            return View(subjectType);
        }

        // POST: SubjectTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] SubjectType subjectType)
        {
            if (id != subjectType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subjectType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubjectTypeExists(subjectType.Id))
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
            return View(subjectType);
        }

        // GET: SubjectTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subjectType = await _context.SubjectType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (subjectType == null)
            {
                return NotFound();
            }

            return View(subjectType);
        }

        // POST: SubjectTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subjectType = await _context.SubjectType.FindAsync(id);
            _context.SubjectType.Remove(subjectType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubjectTypeExists(int id)
        {
            return _context.SubjectType.Any(e => e.Id == id);
        }
    }
}

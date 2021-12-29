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
    public class NotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public NotesController(ApplicationDbContext context,UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Notes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Note.ToListAsync());
        }
        public async Task<IActionResult> GetStudentNotesAsStudent()
        {
            var user = await GetCurrentUserAsync();
            var notes = _context.Note.Where(w => w.StudentId == user.Id).ToList();
            foreach (var note in notes)
            {
                note.Subject = _context.Subject.Where(w => w.Id == note.SubjectId).FirstOrDefault();
                note.Subject.SubjectType = _context.SubjectType.Where(w => w.Id == note.Subject.SubjectTypeId).FirstOrDefault();
            }
            var subjects = _context.Note.Where(w => w.StudentId == user.Id).Select(s => s.Subject).ToList();
            Dictionary<int, string> subjectNames = new Dictionary<int, string>();
            for (int i = 0; i < subjects.Count; i++)
            {
                subjects[i].SubjectType = _context.SubjectType.Where(w => w.Id == subjects[i].SubjectTypeId).FirstOrDefault();
                subjectNames.Add(i, subjects[i].SubjectType.Name);
            }

            ViewBag.SubjectNames = subjectNames;
            return View("Index", notes);
        }
        public async Task<IActionResult> GetStudentNotesAsParent(string id)
        {
            var notes = _context.Note.Where(w => w.StudentId == id).ToList();
            foreach (var note in notes)
            {
                note.Subject= _context.Subject.Where(w => w.Id == note.SubjectId).FirstOrDefault();
                note.Subject.SubjectType= _context.SubjectType.Where(w => w.Id == note.Subject.SubjectTypeId).FirstOrDefault();
            }
            var subjects = _context.Note.Where(w => w.StudentId == id).Select(s => s.Subject).ToList();
            Dictionary<int, string> subjectNames = new Dictionary<int, string>();
            for (int i = 0; i < subjects.Count; i++)
            {
                subjects[i].SubjectType = _context.SubjectType.Where(w => w.Id == subjects[i].SubjectTypeId).FirstOrDefault();
                subjectNames.Add(i, subjects[i].SubjectType.Name);
            }

            ViewBag.SubjectNames = subjectNames;
            return View("Index",notes);
        }

        // GET: Notes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // GET: Notes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Notes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,Id,StudentId,DateTime")] Note note)
        {
            if (ModelState.IsValid)
            {
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(note);
        }

        // GET: Notes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Text,Id,StudentId,DateTime")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(note);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteExists(note.Id))
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
            return View(note);
        }

        // GET: Notes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Note
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Note.FindAsync(id);
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Note.Any(e => e.Id == id);
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}

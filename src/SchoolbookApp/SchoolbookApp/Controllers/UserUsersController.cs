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
    public class UserUsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserUsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserUsers
        public async Task<IActionResult> Index(string? id)
        {
            return View(await _context.UserUser.Where(x => x.UserId == id).ToListAsync());
        }

        // GET: UserUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userUser = await _context.UserUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userUser == null)
            {
                return NotFound();
            }

            return View(userUser);
        }

        // GET: UserUsers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,StudentId")] UserUser userUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userUser);
        }

        // GET: UserUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userUser = await _context.UserUser.FindAsync(id);
            if (userUser == null)
            {
                return NotFound();
            }
            return View(userUser);
        }

        // POST: UserUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,StudentId")] UserUser userUser)
        {
            if (id != userUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserUserExists(userUser.Id))
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
            return View(userUser);
        }

        // GET: UserUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userUser = await _context.UserUser
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userUser == null)
            {
                return NotFound();
            }

            return View(userUser);
        }

        // POST: UserUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userUser = await _context.UserUser.FindAsync(id);
            _context.UserUser.Remove(userUser);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserUserExists(int id)
        {
            return _context.UserUser.Any(e => e.Id == id);
        }
    }
}

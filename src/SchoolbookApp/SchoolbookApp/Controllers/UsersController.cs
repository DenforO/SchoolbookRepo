using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolbookApp.Data;
using SchoolbookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: UsersController
        public ActionResult Index(string? name, string? email)
        {
            if (name == null && email == null)
            {
                return View(_userManager.Users.ToList());
            }
            else
            {
                return View(_userManager.Users
                                            .ToList()
                                            .Where(x =>
                                                (name != null && x.Name.ToLower().Contains(name.ToLower())) ||
                                                (name != null && x.Surname.ToLower().Contains(name.ToLower())) ||
                                                (email != null && x.Email.ToLower().Contains(email.ToLower()))));
            }
        }

        // GET: UsersController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UsersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.SchoolClasses = _context.SchoolClass.ToList();
            ViewBag.SchoolClass = _userManager.FindByIdAsync(id).Result.SchoolClass;
            ViewBag.Role = _userManager.GetRolesAsync(_userManager.FindByIdAsync(id).Result).Result.Single();
            ViewBag.Children = _userManager.Users
                                                .Join(_context.UserUser,
                                                      x => x.Id,
                                                      y => y.StudentId,
                                                      (x, y) => new { x, y.UserId })
                                                .Where(student => student.UserId == id)
                                                .Select(student => student.x)
                                                .ToList();


            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Surname")] ApplicationUser user, int schoolClassNum, char schoolClassLetter)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedUser = _userManager.FindByIdAsync(id).Result;
                    updatedUser.Name = user.Name;
                    updatedUser.Surname = user.Surname;
                    updatedUser.SchoolClass = _context.SchoolClass.Where(x => x.Num == schoolClassNum && x.Letter == schoolClassLetter).Single();
                    var result = await _userManager.UpdateAsync(updatedUser);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_userManager.FindByIdAsync(id) == null)
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
            return View(user);
        }


        // GET: UsersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UsersController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}

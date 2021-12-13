using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchoolbookApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Controllers
{
    public class ProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProfilesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return RedirectToAction("TeacherMain", "Profiles");
        }

        public async Task<IActionResult> TeacherMain()
        {
            IdentityUser usr = await GetCurrentUserAsync();

            //всички часове, които води логнатият учител
            var subjectsByTeacher = _context.Subject.Where(x => x.TeacherId == usr.Id).ToList();

            //Изпраща предметите, по които учителят преподава, към фронтенда
            ViewBag.SubjectTypes = _context.Subject.Where(x => x.TeacherId == usr.Id) //всички часове, които води логнатият учител
                                                    .Select(x => x.SubjectType)       //избира само предмета, по който е даденият час
                                                    .Distinct()                       //за повтарящи се предмети (Математика на 1 А, Математика на 7 Б...)
                                                    .ToList();

            //Изпраща към фронтенда свързана таблица с кокнкретните предмети (във фронтенда - Item1), водени от учителя, и класовете (във фронтенда - Item2), на които ги води.
            ViewBag.SchoolClasses = _context.SchoolClass.ToList().Join(
                                        subjectsByTeacher,
                                        x => x.Id,
                                        y => y.SchoolClassId,
                                        (x, y) => (x, y)).ToList();
            return View("TeacherMain");
        }


        public async Task<IActionResult> TeacherClass()
        {
            return View("TeacherClass");
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}

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
                        
            var subjectsByTeacher = _context.Subject.Where(x => x.TeacherId == usr.Id).ToList();
            var normalizedName = _context.Users.Where(x => x.Id == usr.Id).Select(x => x.NormalizedUserName).FirstOrDefault(); // предстои да се взимат имената
            var isMainTeacher = true;
            var mainClass = _context.SchoolClass.Where(x => x.Id == 21) // хардкоудната стойност засега
                                                .Select(x => new
                                                {
                                                    Id = x.Id,
                                                    ClassName = x.Num + " " + x.Letter
                                                })
                                                .FirstOrDefault();
            var gradesList = _context.Grade.Select(x => x.Value).ToList();
            var avgScore = gradesList.Sum()/gradesList.Count; //Да се вземе средната оценка за даден клас

            ViewBag.SubjectTypes = _context.Subject.Where(x => x.TeacherId == usr.Id)
                                                    .Select(x => x.SubjectType)      
                                                    .Distinct()                      
                                                    .ToList();
                       
            ViewBag.SchoolClasses = _context.SchoolClass.ToList().Join(
                                        subjectsByTeacher,
                                        x => x.Id,
                                        y => y.SchoolClassId,
                                        (x, y) => (x, y)).ToList();

            ViewBag.Name = normalizedName;
            ViewBag.IsMainTeacher = isMainTeacher;
            ViewBag.MainClassId = mainClass.Id;
            ViewBag.MainClassName = mainClass.ClassName;
            ViewBag.AvgScore = avgScore;

            return View("TeacherMain");
        }


        public async Task<IActionResult> TeacherClass()
        {
            return View("TeacherClass");
        }

        private Task<IdentityUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SchoolbookApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolbookApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser usr = await GetCurrentUserAsync();

            if(usr == null)
            {
                return View();
            }
            else if (_userManager.IsInRoleAsync(usr, "Admin").Result)
            {
                return RedirectToRoute(new { action = "AdminMain", controller = "Profiles" });
            }
            else if (_userManager.IsInRoleAsync(usr, "Student").Result)
            {
                return RedirectToRoute(new { action = "StudentMain", controller = "Profiles" });
            }
            else if (_userManager.IsInRoleAsync(usr, "Parent").Result)
            {
                return RedirectToRoute(new { action = "ParentMain", controller = "Profiles" });
            }
            else
            {
                return RedirectToRoute(new { action = "TeacherMain", controller = "Profiles" });
            }
        }

        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SchoolbookApp.Data;
using SchoolbookApp.Models;

namespace SchoolbookApp.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;
        //private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
            //_emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Required]
            [Display(Name = "PhoneNumber")]
            public string PhoneNumber { get; set; }

            [Required]
            [Display(Name = "Role")]
            public string Role { get; set; }

            [Display(Name = "ClassNum")]
            public int ClassNum { get; set; }


            [Display(Name = "ClassLetter")]
            public string ClassLetter { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email, Name=Input.Name,Surname=Input.Surname,PhoneNumber=Input.PhoneNumber,SchoolClassId=27};
                
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _userManager.AddToRoleAsync(user, Input.Role);
                    if(Input.Role == "Student")
                    {
                        EnrollStudentInClass(user);
                    }
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        //await _signInManager.SignInAsync(user, isPersistent: false);
                        //if (await _userManager.IsInRoleAsync(user, "Student"))
                        //{
                        //    returnUrl ??= Url.Content("~/Profiles/StudentMain");
                        //}
                        //else if (await _userManager.IsInRoleAsync(user, "Teacher"))
                        //{
                        //    returnUrl ??= Url.Content("~/Profiles/TeacherMain");
                        //}
                        //else if (await _userManager.IsInRoleAsync(user, "Parent"))
                        //{
                        //    returnUrl ??= Url.Content("~/");
                        //}
                        //else if (await _userManager.IsInRoleAsync(user, "Admin"))
                        //{
                        //    returnUrl ??= Url.Content("~/");
                        //}
                        returnUrl ??= Url.Content("~/Profiles/AdminMain");
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private void EnrollStudentInClass(ApplicationUser user)
        {
            int schoolClassNum = Input.ClassNum;
            char schoolClassLetter = char.Parse(Input.ClassLetter);
            var schoolClassId = _context.SchoolClass.Where(w => w.Num == schoolClassNum && w.Letter == schoolClassLetter)
                .Select(s => s.Id)
                .FirstOrDefault();
            //UserSchoolClass userSchoolClass = new UserSchoolClass();
            //userSchoolClass.UserId = user.Id;
            //userSchoolClass.SchoolClassId = schoolClassId;
            user.SchoolClassId = schoolClassId;
            //_context.UserSchoolClass.Add(userSchoolClass);
            _context.SaveChanges();
        }
    }
}

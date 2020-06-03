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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using miniProjet.Data;
using miniProjet.Model;

namespace miniProjet.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public RegisterModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<RegisterModel> logger,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _roleManager = roleManager;
            _db = db;
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
            [Display(Name ="Prénom")]
            public string firstName { get; set; }
            [Required]
            [Display(Name = "Nom")]
            public string LastName { get; set; }
            [Required]
            [Display(Name = "Type")]
            public string role { get; set; }
           
        }
        public IEnumerable<SelectListItem> roles = new[]
           {
                new SelectListItem{Value="Professeur", Text="Professeur"},
                new SelectListItem{Value="Etudiant", Text="Etudiant"},
            };
        public async Task OnGetAsync(string returnUrl = null)
        {
            
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = Input.firstName+" "+Input.LastName, Email = Input.Email, role=Input.role, firstName=Input.firstName, lastName=Input.LastName};
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    if(!await _roleManager.RoleExistsAsync(GestionRole.AdminUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(GestionRole.AdminUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(GestionRole.StudentUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(GestionRole.StudentUser));
                    }
                    if (!await _roleManager.RoleExistsAsync(GestionRole.TeacherUser))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(GestionRole.TeacherUser));
                    }
                    if (Input.role.Equals("Etudiant"))
                    {
                        await _userManager.AddToRoleAsync(user, GestionRole.StudentUser);
                    }
                    else if (Input.role.Equals("Professeur"))
                    {
                        await _userManager.AddToRoleAsync(user, GestionRole.TeacherUser);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, GestionRole.AdminUser);
                    }
                    _logger.LogInformation("User created a new account with password.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email });
                    }
                    else
                    {
                        return LocalRedirect("~/Administration/Users");
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
    }
}

using AppointmentScheduling.Data;
using AppointmentScheduling.Models;
using AppointmentScheduling.Models.ViewModels;
using AppointmentScheduling.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace AppointmentScheduling.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private  UserManager<ApplicationUsers> _userManager;
        private  SignInManager<ApplicationUsers> _signInManager;
        private  RoleManager<IdentityRole> _roleManager;

        public AccountController(ApplicationDbContext db, UserManager<ApplicationUsers> userManager,SignInManager<ApplicationUsers>signInManager,RoleManager<IdentityRole> roleManager)
        {
            
            _userManager = userManager;
            _signInManager=signInManager;
            _roleManager= roleManager;
        }

        
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            //var user = await _userManager.FindByNameAsync(model.Email);
            //if (user != null)
            //{
            //    HttpContext.Session.SetString("ssusername", user.Name);               //code for session 
            //}
            //HttpContext.Session.GetString("ssusername");--taking the session data in controleer.
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Appointment");
                    // Success logic (e.g., redirect to dashboard)
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
               
            }
            ModelState.AddModelError("", "Invalid credentials");
            return View(model);
        }
        public async Task<IActionResult> Register()
        {
            if( ! await _roleManager.RoleExistsAsync(Helper.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Admin));
            }
            if (!await _roleManager.RoleExistsAsync(Helper.Doctor))
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Doctor));
            }
            if(!await _roleManager.RoleExistsAsync(Helper.Patient))
            {
                await _roleManager.CreateAsync(new IdentityRole(Helper.Patient));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUsers
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                };
                var result=await _userManager.CreateAsync(user,model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.RoleName);
                    if(!User.IsInRole(Helper.Admin))
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    else
                    {
                        TempData["NewsAdminRegister"] = user.Name;
                    }
                        return RedirectToAction("Index", "Appointment");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
    }
}

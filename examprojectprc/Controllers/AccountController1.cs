using examprojectpr.Core.Models;
using examprojectpr.Data.DAL;
using examprojectprc.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.Entity;

namespace examprojectprc.Controllers
{
    public class AccountController1 : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbcontext _context;
        public AccountController1(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<AppUser> signInManager, AppDbcontext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(MemberRegisterViewModel memberRegisterVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = null;

            user = await _userManager.FindByNameAsync(memberRegisterVM.Username);
            if (user is not null)
            {
                ModelState.AddModelError("Username", "UserName already exist!");
                return View();
            }
            user = await _userManager.FindByEmailAsync(memberRegisterVM.Email);

            if (user is not null)
            {
                ModelState.AddModelError("Email", "Email already exist!");
                return View();
            }

            AppUser appUser = new AppUser
            {
                FullName = memberRegisterVM.Fullname,
                UserName = memberRegisterVM.Username,
                Email = memberRegisterVM.Email,
                BirthDate = memberRegisterVM.Birthdate
            };

            var result = await _userManager.CreateAsync(appUser, memberRegisterVM.Password);

            if (result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }


            await _userManager.AddToRoleAsync(appUser, "Member");
            await _signInManager.SignInAsync(appUser, isPersistent: false);

            return RedirectToAction("index", "Home");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(MemberLoginViewModel memberLoginVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = null;

            user = await _userManager.FindByNameAsync(memberLoginVM.Username);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid Username or Password or Email!");
                return View();
            }


            var result = await _signInManager.PasswordSignInAsync(user, memberLoginVM.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid UserName or password and email!");
                return View();
            }

            return RedirectToAction("Index", "Home");

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
        [Authorize(Roles = "Member,  Admin, SuperAdmin")]
        public async Task<IActionResult> Profile()
        {
            AppUser appUser = null;

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                appUser = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
            }


            List<Order> orders = await _context.Orders.Where(x => x.AppUserId == appUser.Id).ToListAsync();

            return View(orders);
        }
    }
}

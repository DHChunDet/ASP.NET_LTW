using DoAnLTW.Data;
using DoAnLTW.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace DoAnLTW.Controllers
{
    public class HomeController : Controller
    {
		private readonly DoAnLTWContext _context;

		public HomeController(DoAnLTWContext context)
		{
			_context = context;
		}

		public IActionResult Index()
        {
            return View(_context.Product.Include(p => p.category).ToList());
        }

        public ActionResult Register()
        {
            return View();
        }
		[HttpPost]
		public ActionResult Register(User user)
		{
			_context.User.Add(user);
			_context.SaveChanges();
			return View("Login");
		}
		//-------------------------------------------------------------------------------
		public IActionResult Login()
		{
			return View();
		}
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync();
			return View("Login");
		}
		[HttpPost]
		public IActionResult Login(string name, string password)
		{
			var user = _context.User.Where(u => u.Name == name && u.Password == password).FirstOrDefault<User>();
			if (user == null || _context.User == null)
			{
				return View();
			}
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.Email),
				new Claim(ClaimTypes.Role, user.Role),
			};
			var claimsIdentity = new ClaimsIdentity(
			claims, CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			new ClaimsPrincipal(claimsIdentity)
			);
			return RedirectToAction("Index", "Home");
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
    }
}

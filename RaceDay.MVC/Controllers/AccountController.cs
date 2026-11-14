using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiClient _api;
        public AccountController(ApiClient api) { _api = api; }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        public async Task<IActionResult> Register(string firstName, string lastName, string email, string password, string phone, string role)
        {
            var ok = await _api.PostAsync("Users/register", new
            {
                lastName,
                email,
                password,
                phone,
                role
            });

            if (ok) return RedirectToAction("Login");
            ViewBag.Error = "Registration failed. Email may already exist.";
            return View();
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _api.PostForObjectAsync<LoginUser>("Users/login", new
            {
                email,
                password
            });

            if (user == null)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            // Save session - MUST match OrganiserController
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("Role", user.Role);
            HttpContext.Session.SetString("Name", user.FirstName + " " + user.LastName);
            HttpContext.Session.SetString("Email", user.Email);

            if (user.Role == "Organiser")
                return RedirectToAction("Dashboard", "Organiser");

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // Helper class for login response from API
        public class LoginUser
        {
            public int UserId { get; set; }
            public string FirstName { get; set; } = "";
            public string LastName { get; set; } = "";
            public string Email { get; set; } = "";
            public string Role { get; set; } = "";
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;

namespace RaceDay.MVC.Controllers
{
    public class OrganiserController : Controller
    {
        private readonly ApiClient _api;
        public OrganiserController(ApiClient api) { _api = api; }

        private bool IsOrganiser()
        {
            return HttpContext.Session.GetString("Role") == "Organiser";
        }

        // GET: /Organiser - redirect to Dashboard so /Organiser never 404s
        public IActionResult Index()
        {
            return RedirectToAction("Dashboard");
        }

        // GET: /Organiser/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            var events = await _api.GetAsync<List<object>>("Events");
            ViewBag.Events = events;
            return View();
        }

        // GET: /Organiser/CreateEvent
        [HttpGet]
        public IActionResult CreateEvent()
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: /Organiser/CreateEvent
        [HttpPost]
        public async Task<IActionResult> CreateEvent(string title, string description, DateTime eventDate, string location, string eventType, int categoryId)
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            var userIdStr = HttpContext.Session.GetString("UserId");
            int.TryParse(userIdStr, out int organiserId);

            var ok = await _api.PostAsync("Events", new
            {
                title,
                description,
                eventDate,
                location,
                eventType,
                categoryId,
                organiserId
            });

            if (ok) return RedirectToAction("Dashboard");
            ViewBag.Error = "Failed to create event. Check API is running.";
            return View();
        }

        // GET: /Organiser/Results?eventId=1
        public async Task<IActionResult> Results(int eventId)
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            var enrols = await _api.GetAsync<List<object>>($"Enrolments/event/{eventId}");
            ViewBag.Enrols = enrols;
            ViewBag.EventId = eventId;
            return View();
        }

        // POST: /Organiser/SaveResult
        [HttpPost]
        public async Task<IActionResult> SaveResult(int enrolmentId, string finishTime, int position)
        {
            if (!IsOrganiser()) return RedirectToAction("Login", "Account");

            await _api.PostAsync("Results", new
            {
                enrolmentId,
                finishTime,
                position
            });

            return RedirectToAction("Dashboard");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
namespace RaceDay.MVC.Controllers
{
    public class ParticipantController : Controller
    {
        private readonly ApiClient _api;
        public ParticipantController(ApiClient api) { _api = api; }
        public async Task<IActionResult> MyEvents() { var list = await _api.GetAsync<List<object>>("Enrolments/mine/1"); return View(list); }
        public async Task<IActionResult> Enrol(int eventId) { await _api.PostAsync("Enrolments", new { UserId = 1, EventId = eventId, CategoryId = 1 }); return RedirectToAction("MyEvents"); }
        public async Task<IActionResult> MyResults() { var data = await _api.GetAsync<object>("Results/my/1"); ViewBag.Data = data; return View(); }
    }
}
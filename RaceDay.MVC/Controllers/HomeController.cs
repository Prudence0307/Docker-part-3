using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RaceDay.MVC.Services;
namespace RaceDay.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiClient _api;
        public HomeController(ApiClient api) { _api = api; }
        public async Task<IActionResult> Index(string? type) { var url = string.IsNullOrEmpty(type) ? "Events" : $"Events?type={type}"; 
           var events = await _api.GetAsync<List<JsonElement>>(url); ViewBag.Type = type; return View(events); }
    }
}

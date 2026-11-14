using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Services;
namespace RaceDay.API.Controllers
{
  [Route("api/[controller]")] [ApiController]
  public class ResultsController : ControllerBase
  {
    private readonly RaceDayDbContext _context;
    public ResultsController(RaceDayDbContext context) { _context = context; }

    [HttpGet] public async Task<IActionResult> GetAll() =>
      Ok(await _context.Results.Include(r => r.Enrolment).ThenInclude(e => e!.User)
        .Include(r => r.Enrolment).ThenInclude(e => e!.Event).ToListAsync());

    // Only an Organiser can capture results
    [HttpPost] public async Task<IActionResult> Create(Result r)
    {
      if (CurrentUser.Role(HttpContext) != "Organiser") return StatusCode(403, "Only Organiser can capture results");
      _context.Results.Add(r); await _context.SaveChangesAsync(); return Ok(r);
    }

    [HttpGet("my/{userId}")] public async Task<IActionResult> MyResults(int userId)
    {
      var results = await _context.Results.Include(r => r.Enrolment).ThenInclude(e => e!.Event)
        .Where(r => r.Enrolment!.UserId == userId).ToListAsync();
      var total = await _context.Enrolments.CountAsync();
      return Ok(new { results, totalParticipants = total });
    }
  }
}

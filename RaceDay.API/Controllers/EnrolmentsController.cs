using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;
namespace RaceDay.API.Controllers
{
  [Route("api/[controller]")] [ApiController]
  public class EnrolmentsController : ControllerBase
  {
    private readonly RaceDayDbContext _context;
    public EnrolmentsController(RaceDayDbContext context) { _context = context; }

    [HttpGet] public async Task<IActionResult> GetAll() =>
      Ok(await _context.Enrolments.Include(e => e.User).Include(e => e.Event).ToListAsync());

    [HttpGet("mine/{userId}")] public async Task<IActionResult> GetMine(int userId) =>
      Ok(await _context.Enrolments.Include(e => e.Event).Where(e => e.UserId == userId).ToListAsync());

    [HttpGet("event/{eventId}")] public async Task<IActionResult> GetForEvent(int eventId) =>
      Ok(await _context.Enrolments.Include(e => e.User).Where(e => e.EventId == eventId).ToListAsync());

    [HttpPost] public async Task<IActionResult> Enrol(EnrolDto dto)
    {
      if (await _context.Enrolments.AnyAsync(x => x.UserId == dto.UserId && x.EventId == dto.EventId)) return Conflict("Already enrolled");
      var en = new Enrolment { UserId = dto.UserId, EventId = dto.EventId, CategoryId = dto.CategoryId };
      _context.Enrolments.Add(en); await _context.SaveChangesAsync(); return Ok(en);
    }
  }
}

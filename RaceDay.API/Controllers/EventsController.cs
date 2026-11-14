using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using RaceDay.API.Services;
namespace RaceDay.API.Controllers
{
  [Route("api/[controller]")] [ApiController]
  public class EventsController : ControllerBase
  {
    private readonly RaceDayDbContext _context;
    public EventsController(RaceDayDbContext context) { _context = context; }
    private bool IsOrganiser() => CurrentUser.Role(HttpContext) == "Organiser";

    [HttpGet] public async Task<IActionResult> GetAll(string? type)
    {
      var q = _context.Events.Include(e => e.Category).AsQueryable();
      if (!string.IsNullOrEmpty(type)) q = q.Where(e => e.EventType == type);
      return Ok(await q.ToListAsync());
    }

    [HttpGet("{id}")] public async Task<IActionResult> GetOne(int id)
    {
      var ev = await _context.Events.Include(e => e.Category).FirstOrDefaultAsync(e => e.EventId == id);
      return ev == null ? NotFound() : Ok(ev);
    }

    // The first check is strict, so any non-Organiser gets 403
    [HttpPost] public async Task<IActionResult> Create(Event ev)
    {
      if (!IsOrganiser()) return StatusCode(403, "Only Organiser can create events");
      ev.OrganiserId = CurrentUser.Id(HttpContext) ?? ev.OrganiserId;
      _context.Events.Add(ev); await _context.SaveChangesAsync();
      return CreatedAtAction(nameof(GetOne), new { id = ev.EventId }, ev);
    }

    [HttpPut("{id}")] public async Task<IActionResult> Update(int id, Event ev)
    { if (!IsOrganiser()) return StatusCode(403); ev.EventId = id; _context.Events.Update(ev); await _context.SaveChangesAsync(); return Ok(ev); }

    [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id)
    { if (!IsOrganiser()) return StatusCode(403); var e = await _context.Events.FindAsync(id); if (e == null) return NotFound(); _context.Events.Remove(e); await _context.SaveChangesAsync(); return Ok(); }
  }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
namespace RaceDay.API.Controllers
{
  [Route("api/[controller]")] [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly RaceDayDbContext _context;
    public UsersController(RaceDayDbContext context) { _context = context; }
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _context.Users.ToListAsync());
    [HttpGet("{id}")] public async Task<IActionResult> GetOne(int id) => Ok(await _context.Users.FindAsync(id));
    [HttpPut("{id}")] public async Task<IActionResult> Update(int id, User u) { u.UserId = id; _context.Users.Update(u); await _context.SaveChangesAsync(); return Ok(u); }
  }
}

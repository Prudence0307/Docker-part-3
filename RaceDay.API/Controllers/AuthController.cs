using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.DTOs;
using RaceDay.API.Models;
namespace RaceDay.API.Controllers
{
  [Route("api/[controller]")] [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly RaceDayDbContext _context;
    public AuthController(RaceDayDbContext context) { _context = context; }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
      if (await _context.Users.AnyAsync(u => u.Email == dto.Email)) return BadRequest("Email already exists");
      var user = new User { FirstName = dto.FirstName, LastName = dto.LastName, Email = dto.Email,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password), Role = dto.Role, Phone = dto.Phone };
      _context.Users.Add(user); await _context.SaveChangesAsync(); return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
      var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
      if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return Unauthorized("Invalid login");
      HttpContext.Session.SetInt32("UserId", user.UserId);
      HttpContext.Session.SetString("Role", user.Role);
      HttpContext.Session.SetString("FullName", user.FullName);
      return Ok(user);
    }

    [HttpPost("logout")] public IActionResult Logout() { HttpContext.Session.Clear(); return Ok("Logged out"); }

    [HttpGet("me")] public IActionResult Me()
    {
      var id = HttpContext.Session.GetInt32("UserId"); if (id == null) return Unauthorized();
      return Ok(new { UserId = id, Role = HttpContext.Session.GetString("Role"), FullName = HttpContext.Session.GetString("FullName") });
    }
  }
}

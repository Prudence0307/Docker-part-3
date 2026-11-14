using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
namespace RaceDay.API.Controllers
{
  [Route("api/[controller]")] [ApiController]
  public class CategoriesController : ControllerBase
  {
    private readonly RaceDayDbContext _context;
    public CategoriesController(RaceDayDbContext context) { _context = context; }
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _context.Categories.ToListAsync());
    [HttpPost] public async Task<IActionResult> Create(Category c) { _context.Categories.Add(c); await _context.SaveChangesAsync(); return Ok(c); }
  }
}

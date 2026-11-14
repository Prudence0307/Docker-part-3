using Microsoft.EntityFrameworkCore;
using RaceDay.API.Models;
namespace RaceDay.API.Data
{
  public class RaceDayDbContext : DbContext
  {
    public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Enrolment> Enrolments { get; set; }
    public DbSet<Result> Results { get; set; }

    // Seed 3 categories, because CreateEvent uses CategoryId = 1 and the foreign key fails without it
    protected override void OnModelCreating(ModelBuilder b)
    {
      b.Entity<Category>().HasData(
        new Category { CategoryId = 1, Name = "5km", Description = "Fun run" },
        new Category { CategoryId = 2, Name = "10km", Description = "Road race" },
        new Category { CategoryId = 3, Name = "21km", Description = "Half marathon" });
    }
  }
}

using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;
using Xunit;
namespace RaceDay.API.Tests
{
  public class AuthTests
  {
    private RaceDayDbContext GetDb()
    {
      var o = new DbContextOptionsBuilder<RaceDayDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
      return new RaceDayDbContext(o);
    }

    public async Task Register_Creates_User()
    {
      var db = GetDb();
      db.Users.Add(new User { FirstName = "Jarred", LastName = "Test", Email = "j@t.com", PasswordHash = "123", Role = "Participant" });
      await db.SaveChangesAsync();
      Assert.Equal(1, await db.Users.CountAsync());
    }

    [Fact] public void Login_Password_Verify_Works()
    {
      var hash = BCrypt.Net.BCrypt.HashPassword("password123");
      Assert.True(BCrypt.Net.BCrypt.Verify("password123", hash));
    }

    public async Task Enrol_Prevents_Duplicate_Logic()
    {
      var db = GetDb();
      db.Enrolments.Add(new Enrolment { UserId = 1, EventId = 1 });
      await db.SaveChangesAsync();
      Assert.True(await db.Enrolments.AnyAsync(x => x.UserId == 1 && x.EventId == 1));
    }

     public async Task Event_Filter_By_Type_Works()
    {
      var db = GetDb();
      db.Events.Add(new Event { Title = "Run", EventType = "Run", EventDate = DateTime.Now, Location = "Polokwane" });
      db.Events.Add(new Event { Title = "Walk", EventType = "Walk", EventDate = DateTime.Now, Location = "Polokwane" });
      await db.SaveChangesAsync();
      Assert.Equal(1, await db.Events.CountAsync(e => e.EventType == "Run"));
    }

    public async Task Result_Position_Calculation()
    {
      var db = GetDb();
      db.Results.Add(new Result { EnrolmentId = 1, Position = 47, TotalFinishers = 312 });
      await db.SaveChangesAsync();
      var r = await db.Results.FirstAsync();
      Assert.Equal(47, r.Position);
      Assert.Equal(312, r.TotalFinishers);
    }
  }
}

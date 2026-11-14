using System.ComponentModel.DataAnnotations;
namespace RaceDay.API.Models
{
  public class Event
  {
    public int EventId { get; set; }
    [Required] public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    [Required] public DateTime EventDate { get; set; }
    [Required] public string Location { get; set; } = "";
    [Required] public string EventType { get; set; } = "Run"; // Run, Walk, Cycle
    public string BannerImageUrl { get; set; } = "";
    public string BannerUrl { get; set; } = "";
    public int OrganiserId { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
  }
}

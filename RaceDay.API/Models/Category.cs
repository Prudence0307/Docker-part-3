using System.ComponentModel.DataAnnotations;
namespace RaceDay.API.Models
{
  public class Category
  {
    public int CategoryId { get; set; }
    [Required] public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public ICollection<Event> Events { get; set; } = new List<Event>();
  }
}

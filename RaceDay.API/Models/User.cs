using System.ComponentModel.DataAnnotations;
namespace RaceDay.API.Models
{
  public class User
  {
    public int UserId { get; set; }
    [Required] public string FirstName { get; set; } = "";
    [Required] public string LastName { get; set; } = "";
    public string FullName => FirstName + " " + LastName;
    [Required, EmailAddress] public string Email { get; set; } = "";
    [Required] public string PasswordHash { get; set; } = "";
    [Required] public string Role { get; set; } = "Participant";
    public string Phone { get; set; } = "";
    public ICollection<Enrolment> Enrolments { get; set; } = new List<Enrolment>();
  }
}

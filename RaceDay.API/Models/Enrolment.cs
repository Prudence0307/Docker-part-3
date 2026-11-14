namespace RaceDay.API.Models
{
  public class Enrolment
  {
    public int EnrolmentId { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public int EventId { get; set; }
    public Event? Event { get; set; }
    public int CategoryId { get; set; }
    public string Status { get; set; } = "Confirmed";
    public DateTime EnrolledDate { get; set; } = DateTime.Now;
    public Result? Result { get; set; }
  }
}

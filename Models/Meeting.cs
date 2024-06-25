namespace QLGB.API.Models;

public class Meeting
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public DateTime Date { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Attendee>? Attendees { get; set; }
}

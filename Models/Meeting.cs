namespace QLGB.API.Models;

public class Meeting
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Attendee>? Attendees { get; set; }
}

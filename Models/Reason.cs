namespace QLGB.API.Models;

public class Reason
{
    public int ReasonId { get; set; }
    public string? Title { get; set;}
    public bool IsActive { get; set; }
    public ICollection<Attendee>? Attendees { get; set; }
}

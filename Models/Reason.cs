namespace QLGB.API.Models;

public class Reason
{
    public int Id { get; set; }
    public string? Name { get; set;}
    public bool IsActive { get; set; }
    public ICollection<Attendee>? Attendees { get; set; }
}

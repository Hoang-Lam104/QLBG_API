namespace QLGB.API.Models;

public class Room
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public ICollection<Attendee>? Attendees { get; set; }
}

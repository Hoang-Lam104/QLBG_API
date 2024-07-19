namespace QLGB.API.Models;

public class Room
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public bool IsActive { get; set; }
    public ICollection<Attendee>? Attendees { get; set; }
}

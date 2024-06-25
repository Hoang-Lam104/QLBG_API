namespace QLGB.API.Models;

public class Attendee
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MeetingId { get; set; }
    public int? RoomId { get; set; }
    public string? Status { get; set; }
    public string? Reason { get; set; } 
    public DateTime RegisterTime { get; set; }
    public DateTime MeetingTime { get; set; }
    public User? User { get; set; }
    public Meeting? Meeting { get; set; }
    public Room? Room { get; set; }
}

namespace QLGB.API.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string Fullname { get; set; }
    public required string Position { get; set; }
    public int DepartmentId { get; set; }
    public ICollection<Attendee>? Attendees { get; set; }
    public Department? Department { get; set; }
    public ICollection<Log>? Logs { get; set; }
}

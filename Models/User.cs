namespace QLGB.API.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string Fullname { get; set; } = "";
    public string Position { get; set; } = "";
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Attendee>? Attendees { get; set; }
    public Department? Department { get; set; }
    public ICollection<Log>? Logs { get; set; }
}

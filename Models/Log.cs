namespace QLGB.API.Models;

public class Log
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? DeviceInfo { get; set; }
    public DateTime LogTime { get; set; }
    public string? LogEvent { get; set; }
    public User? User { get; set; }
}

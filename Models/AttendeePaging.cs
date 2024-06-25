using QLGB.API.Dtos;

namespace QLGB.API.Models;

public class AttendeePaging
{
    public List<AttendeeDto>? Attendees { get; set; }
    public int? Total { get; set; }
}

using QLGB.API.Dtos;

namespace QLGB.API.Models;

public class AttenMeetingPagi
{
    public List<AttenMeetingDto>? Meetings { get; set; }
    public int Total { get; set; }
}



namespace QLGB.API.Dtos;

public record class CreateMeetingDtos(
    string Title, 
    DateTime StartTime,
    DateTime EndTime
);
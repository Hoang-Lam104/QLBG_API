namespace QLGB.API.Dtos;

public record class UpdateAttendeeDto(
    int UserId,
    int MeetingId,
    string Status,
    int? RoomId,
    string? Reason,
    DateTime? RegisterTime,
    DateTime? MeetingTime
);
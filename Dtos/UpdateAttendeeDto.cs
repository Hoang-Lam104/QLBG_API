namespace QLGB.API.Dtos;

public record class UpdateAttendeeDto(
    int UserId,
    int MeetingId,
    string Status,
    int? RoomId,
    int? ReasonId,
    string? AnotherReason,
    DateTime? RegisterTime,
    DateTime? MeetingTime
);
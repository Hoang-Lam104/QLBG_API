namespace QLGB.API.Dtos;

public record class AttenMeetingDto(
    int Id,
    string Title,
    int? RoomId,
    int? ReasonId,
    string AnotherReason,
    string Status,
    bool IsActive,
    DateTime StartTime,
    DateTime EndTime
);

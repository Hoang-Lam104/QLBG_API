namespace QLGB.API.Dtos;

public record class AttenMeetingDto(
    int Id,
    string Title,
    int? RoomId,
    string Reason,
    string Status,
    bool IsActive,
    DateTime StartTime,
    DateTime EndTime
);

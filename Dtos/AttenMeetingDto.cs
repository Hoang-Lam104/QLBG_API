namespace QLGB.API.Dtos;

public record class AttenMeetingDto(
    int Id,
    string Title,
    int RoomId,
    string Reason,
    bool IsMeeting,
    DateOnly Date
);

namespace QLGB.API.Dtos;

public record class UpdateAttendeeDto(
    int UserId,
    int MeetingId,
    int RoomId,
    bool IsMeeting,
    string Reason
);

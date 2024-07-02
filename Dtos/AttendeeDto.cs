namespace QLGB.API.Dtos;

public record class AttendeeDto(
    int Id,
    string Fullname,
    int DepartmentId,
    string Position,
    int? RoomId,
    string Status,
    int? ReasonId,
    string? AnotherReason
);

namespace QLGB.API.Dtos;

public record class LogDtos(
    int UserId,
    string DeviceInfo,
    DateTime LogTime,
    string LogEvent
);

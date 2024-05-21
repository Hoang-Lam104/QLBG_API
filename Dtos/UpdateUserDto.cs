namespace QLGB.API.Dtos;

public record class UpdateUserDto(
    string Fullname,
    int DepartmentId,
    string Position
);

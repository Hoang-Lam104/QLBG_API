namespace QLGB.API.Dtos;

public record class CreateUserDtos(
    string Fullname,
    string Username,
    string Password,
    int DepartmentId,
    string Position
);
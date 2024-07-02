namespace QLGB.API.Dtos;

public record class ChangePasswordDto(
    int UserId,
    string OldPassword,
    string NewPassword
);

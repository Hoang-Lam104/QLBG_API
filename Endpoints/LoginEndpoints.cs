using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class LoginEndpoints
{
    public static RouteGroupBuilder MapLoginEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/login");

        group.MapPost("/", async (LoginDtos loginDto, AppDbContext dbContext) =>
        {
            var users = await dbContext.Users.ToListAsync();
            var user = users.Find(item => item.Username == loginDto.Username);
            if (user != null && user.Password == loginDto.Password){
                return Results.Ok(user);
            } 
            return Results.Unauthorized();
        });

        return group;
    }
}

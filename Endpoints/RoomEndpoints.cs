using QLGB.API.Data;

namespace QLGB.API.Endpoints;

public static class RoomEndpoints
{
    public static RouteGroupBuilder MapRoomEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/rooms");

        group.MapGet("/", (AppDbContext dbContext) => dbContext.Rooms);

        return group;
    }
}

using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class RoomEndpoints
{
    public static void MapRoomEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/rooms", (AppDbContext dbContext) => dbContext.Rooms.Where(r => r.IsActive))
            .RequireAuthorization();

        endpoints.MapGet("api/rooms/all", (AppDbContext dbContext) => dbContext.Rooms)
            .RequireAuthorization();

        endpoints.MapPost("api/rooms/new", (CreateRoomDtos newRoom, AppDbContext dbContext) =>
        {
            if (dbContext.Rooms.Any(r => r.Name == newRoom.Name))
            {
                return Results.Conflict(new
                {
                    message = "Hội trường đã tồn tại."
                });
            }
            Room room = new()
            {
                Name = newRoom.Name,
                IsActive = true
            };

            dbContext.Rooms.Add(room);
            dbContext.SaveChanges();

            return Results.NoContent();
        }).RequireAuthorization();

        endpoints.MapPut("api/rooms/active/{id}", async (int id, AppDbContext dbContext) =>
        {
            Room? room = dbContext.Rooms.Find(id);

            if (room == null)
            {
                return Results.NotFound();
            }

            room.IsActive = !room.IsActive;

            dbContext.Entry(room).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization();
    }
}

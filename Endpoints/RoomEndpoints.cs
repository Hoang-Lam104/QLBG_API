﻿using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class RoomEndpoints
{
    public static RouteGroupBuilder MapRoomEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/rooms");

        group.MapGet("/", (AppDbContext dbContext) => dbContext.Rooms.Where(r => r.IsActive))
            .RequireAuthorization();

        group.MapPost("/", (CreateRoomDtos newRoom, AppDbContext dbContext) =>
        {
            Room room = new()
            {
                Name = newRoom.Name,
                IsActive = true
            };

            dbContext.Rooms.Add(room);
            dbContext.SaveChanges();

            return Results.NoContent();
        }).RequireAuthorization();

        group.MapPut("/active/{id}", async (int id, AppDbContext dbContext) =>
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

        return group;
    }
}

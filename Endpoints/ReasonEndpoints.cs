using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class ReasonEndpoints
{
    public static RouteGroupBuilder MapReasonEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/reasons");

        group.MapGet("/", (AppDbContext dbContext) => dbContext.Reasons.Where(r => r.IsActive))
            .RequireAuthorization();

        group.MapGet("/all", (AppDbContext dbContext) => dbContext.Reasons)
            .RequireAuthorization();

        group.MapPut("/active/{id}", async (int id, AppDbContext dbContext) =>
        {
            Reason? reason = dbContext.Reasons.Find(id);

            if (reason == null || reason.Title == "Khác")
            {
                return Results.NotFound();
            }

            reason.IsActive = !reason.IsActive;

            dbContext.Entry(reason).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization();

        group.MapPost("/new", (CreateReasonDtos newReason, AppDbContext dbContext) =>
        {
            if (dbContext.Reasons.Any(r => r.Title == newReason.Title))
            {
                return Results.Conflict(new
                {
                    message = "Lý do đã tồn tại."
                });
            }

            Reason reason = new()
            {
                Title = newReason.Title,
                IsActive = true
            };

            dbContext.Reasons.Add(reason);
            dbContext.SaveChanges();

            return Results.NoContent();
        }).RequireAuthorization();

        return group;
    }
}

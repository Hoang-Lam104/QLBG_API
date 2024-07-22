using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class ReasonEndpoints
{
    public static void MapReasonEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/reasons", (AppDbContext dbContext) => dbContext.Reasons.Where(r => r.IsActive))
            .RequireAuthorization();

        endpoints.MapGet("api/reasons/all", (AppDbContext dbContext) => dbContext.Reasons)
            .RequireAuthorization();

        endpoints.MapPut("api/reasons/active/{id}", async (int id, AppDbContext dbContext) =>
        {
            Reason? reason = dbContext.Reasons.Find(id);

            if (reason == null || reason.Name == "Khác")
            {
                return Results.NotFound();
            }

            reason.IsActive = !reason.IsActive;

            dbContext.Entry(reason).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization();

        endpoints.MapPost("api/reasons/new", (CreateReasonDtos newReason, AppDbContext dbContext) =>
        {
            if (dbContext.Reasons.Any(r => r.Name == newReason.Title))
            {
                return Results.Conflict(new
                {
                    message = "Lý do đã tồn tại."
                });
            }

            Reason reason = new()
            {
                Name = newReason.Title,
                IsActive = true
            };

            dbContext.Reasons.Add(reason);
            dbContext.SaveChanges();

            return Results.NoContent();
        }).RequireAuthorization();
    }
}

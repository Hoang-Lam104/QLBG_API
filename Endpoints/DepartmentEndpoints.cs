using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;
using Microsoft.EntityFrameworkCore;

namespace QLGB.API.Endpoints;

public static class DepartmentEndpoints
{
    public static void MapDepartmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/departments", (AppDbContext dbContext) => dbContext.Departments.Where(d => d.IsActive))
            .RequireAuthorization();

        endpoints.MapGet("api/departments/all", (AppDbContext dbContext) => dbContext.Departments)
            .RequireAuthorization();

        endpoints.MapPost("api/departments/new", (CreateDepartmentDtos newDepartment, AppDbContext dbContext) =>
        {
            if (dbContext.Departments.Any(r => r.Name == newDepartment.Name))
            {
                return Results.Conflict(new
                {
                    message = "Khoa/Phòng đã tồn tại."
                });
            }
            Department department = new()
            {
                Name = newDepartment.Name,
                IsActive = true
            };

            dbContext.Departments.Add(department);
            dbContext.SaveChanges();

            return Results.NoContent();
        }).RequireAuthorization();

        endpoints.MapPut("api/departments/active/{id}", async (int id, AppDbContext dbContext) =>
        {
            Department? department = dbContext.Departments.Find(id);

            if (department == null)
            {
                return Results.NotFound();
            }

            department.IsActive = !department.IsActive;

            dbContext.Entry(department).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization();
    }
}

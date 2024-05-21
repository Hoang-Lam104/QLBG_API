using QLGB.API.Data;

namespace QLGB.API.Endpoints;

public static class DepartmentEndpoints
{
    public static RouteGroupBuilder MapDepartmentEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/departments");

        group.MapGet("/", (AppDbContext dbContext) => dbContext.Departments);

        return group;
    }
}

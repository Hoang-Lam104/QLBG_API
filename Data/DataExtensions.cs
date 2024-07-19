using Microsoft.EntityFrameworkCore;

namespace QLGB.API.Data;

public static class DataExtensions
{
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var DbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        // DbContext.Database.Migrate();
    }
}

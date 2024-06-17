using QLGB.API.Data;
using QLGB.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlite<AppDbContext>("Data Source=DataStore.db");
builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors("MyCors");
app.MapUserEndpoint();
app.MapMeetingEndpoint();
app.MapRoomEndpoint();
app.MapDepartmentEndpoint();
app.MapLoginEndpoint();
app.MigrateDb();

app.Run();
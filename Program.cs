using QLGB.API.Data;
using QLGB.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSqlite<AppDbContext>("Data Source=DataStore.db");
builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    // build.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

app.UseCors("MyCors");
app.MapUserEndpoint();
app.MapMeetingEndpoint();
app.MapRoomEndpoint();
app.MapDepartmentEndpoint();
app.MigrateDb();

app.Run();

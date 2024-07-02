using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(p => p.AddPolicy("MyCors", build =>
{
    build.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAuthentication()
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseCors("MyCors");

app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoint();
app.MapMeetingEndpoint();
app.MapRoomEndpoint();
app.MapReasonEndpoint();
app.MapDepartmentEndpoint();
app.MapLoginEndpoint();
app.MigrateDb();

app.Run();
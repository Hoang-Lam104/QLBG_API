﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class LoginEndpoints
{
    public static void MapLoginEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/api/login", async (
            HttpRequest request,
            LoginDtos loginDto,
            AppDbContext dbContext,
            IConfiguration configuration) =>
        {
            var users = await dbContext.Users.ToListAsync();
            var user = users.Find(item => item.Username == loginDto.Username);
            var password = Convert.ToBase64String(Encoding.UTF8.GetBytes(loginDto.Password));

            if (user != null && user.Password == password && user.IsActive)
            {
                var token = string.Empty;

                var issuer = configuration["Jwt:Issuer"];
                var audience = configuration["Jwt:Audience"];
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));

                var jwtHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);

                var tokenDes = new SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new[] {
                        new Claim("Id", user.Id.ToString()),
                    }),
                    Expires = DateTime.Now.AddHours(6),
                    Audience = audience,
                    Issuer = issuer,
                    SigningCredentials = new SigningCredentials(
                        securityKey,
                        SecurityAlgorithms.HmacSha256Signature
                    )
                };

                var jwtToken = jwtHandler.CreateToken(tokenDes);
                token = jwtHandler.WriteToken(jwtToken);

                UserLogin userLogin = new()
                {
                    UserId = user.Id,
                    Username = loginDto.Username,
                    AccessToken = token
                };

                Log log = new()
                {
                    UserId = user.Id,
                    DeviceInfo = request.Headers.UserAgent,
                    LogTime = DateTime.Now,
                    LogEvent = "Login",
                };

                if (log.DeviceInfo!.Contains("Mozilla"))
                {
                    dbContext.Log.Add(log);
                    await dbContext.SaveChangesAsync();
                }

                return Results.Ok(userLogin);
            }
            else
            {
                return Results.Unauthorized();
            }
        });

        endpoints.MapPost("api/logout/{UserId}", async (
            HttpRequest request,
            int UserId,
            AppDbContext dbContext) =>
        {
            Log log = new()
            {
                UserId = UserId,
                DeviceInfo = request.Headers.UserAgent,
                LogTime = DateTime.Now,
                LogEvent = "Logout",
            };

            if (log.DeviceInfo!.Contains("Mozilla"))
            {
                dbContext.Log.Add(log);
                await dbContext.SaveChangesAsync();
            }

            return Results.Ok(log);
        });
    }
}

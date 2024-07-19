using System.Text;
using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoint(this IEndpointRouteBuilder endpoints)
    {
        // get user's infomations
        endpoints.MapGet("api/user/{id}/info", (int id, AppDbContext dbContext) =>
        {
            User? user = dbContext.Users.Find(id);

            return user is null ? Results.NotFound() : Results.Ok(user);
        }).RequireAuthorization();

        // get list meeting of user by id
        endpoints.MapGet("api/user/{id}/meetings", async (
            int id,
            int pageIndex,
            int numberInPage,
            AppDbContext dbContext,
            string? status,
            int? reasonId,
            int? roomId,
            DateTime? startTime,
            DateTime? endTime) =>
        {
            startTime ??= DateTime.MinValue;
            endTime ??= DateTime.MaxValue;
            status ??= "";
            reasonId ??= 0;
            roomId ??= 0;

            User? user = dbContext.Users.Find(id);

            if (user is null) return Results.NotFound();

            var attendees = await dbContext.Attendees.ToListAsync();
            var list = attendees.Where(attendee => attendee.UserId == id);

            var meetings = new List<AttenMeetingDto>();

            foreach (var item in list)
            {
                var meetingEntity = dbContext.Meetings.Find(item.MeetingId);

                var meetingDto = new AttenMeetingDto
                (
                    Id: meetingEntity!.Id,
                    Title: meetingEntity.Title,
                    RoomId: item.RoomId,
                    ReasonId: item.ReasonId!,
                    AnotherReason: item.AnotherReason!,
                    Status: item.Status!,
                    StartTime: meetingEntity.StartTime,
                    EndTime: meetingEntity.EndTime,
                    IsActive: meetingEntity.IsActive
                );

                if (
                    (status == "" || status == meetingDto.Status) &&
                    (reasonId == 0 || reasonId == meetingDto.ReasonId) &&
                    (roomId == 0 || roomId == meetingDto.RoomId) &&
                    meetingDto.StartTime >= startTime &&
                    meetingDto.EndTime <= endTime &&
                    meetingDto.IsActive
                )
                {
                    meetings.Add(meetingDto);
                }
            }

            AttenMeetingPagi attenMeetingPagi = new()
            {
                Meetings = meetings.OrderByDescending(m => m.Id)
                    .Skip((pageIndex - 1) * numberInPage)
                    .Take(numberInPage)
                    .ToList(),
                Total = meetings.Count()
            };

            return Results.Ok(attenMeetingPagi);
        });

        // register meeting 
        endpoints.MapPut("api/user/attend", async (UpdateAttendeeDto updateAttendee, AppDbContext dbContext) =>
        {
            Attendee? attendee = await dbContext.Attendees.FirstOrDefaultAsync(a =>
                a.UserId == updateAttendee.UserId && a.MeetingId == updateAttendee.MeetingId
            );

            if (attendee is null) return Results.NotFound();

            attendee.RoomId = updateAttendee.RoomId;
            attendee.Status = updateAttendee.Status;
            attendee.ReasonId = updateAttendee.ReasonId;
            attendee.AnotherReason = updateAttendee.ReasonId == 1 ? updateAttendee.AnotherReason : null;
            attendee.RegisterTime = updateAttendee.RegisterTime ?? attendee.RegisterTime;
            attendee.MeetingTime = updateAttendee.MeetingTime ?? attendee.MeetingTime;

            dbContext.Entry(attendee).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization();

        // change user's infomation
        endpoints.MapPut("api/user/{id}/info", async (int id, UpdateUserDto updateUser, AppDbContext dbContext) =>
        {
            User? user = dbContext.Users.Find(id);

            if (user is null || id == 1) return Results.NotFound();

            user.Fullname = updateUser.Fullname;
            user.DepartmentId = updateUser.DepartmentId;
            user.Position = updateUser.Position;

            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.Ok(user);
        }).RequireAuthorization();

        // toggle active user (role: admin)
        endpoints.MapPut("api/user/active/{id}", async (int id, AppDbContext dbContext) =>
        {
            User? user = dbContext.Users.Find(id);

            if (user is null || id == 1) return Results.NotFound();

            user.IsActive = !user.IsActive;

            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization();

        // get list users (role: admin)
        endpoints.MapGet("/api/user/list", (
            int pageIndex,
            int numberInPage,
            AppDbContext dbContext
        ) =>
        {
            return Results.Ok(new
            {
                users = dbContext.Users.OrderBy(u => u.Id).Skip((pageIndex - 1) * numberInPage).Take(numberInPage),
                total = dbContext.Users.Count()
            });
        }
        ).RequireAuthorization();

        // create new user 
        endpoints.MapPost("api/user/new", async (CreateUserDtos newUser, AppDbContext dbContext) =>
        {
            var userItem = dbContext.Users.Any(u => u.Username == newUser.Username);

            if (userItem)
            {
                return Results.Conflict(new
                {
                    messager = "Tài khoản đã tồn tại."
                });
            }

            var password = Convert.ToBase64String(Encoding.UTF8.GetBytes(newUser.Password));

            User user = new()
            {
                Fullname = newUser.Fullname,
                Username = newUser.Username,
                Password = password,
                DepartmentId = newUser.DepartmentId,
                Position = newUser.Position,
                IsActive = true
            };

            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();

            var meetings = await dbContext.Meetings.ToListAsync();

            foreach (var meeting in meetings)
            {
                var attendee = new Attendee
                {
                    UserId = user.Id,
                    MeetingId = meeting.Id,
                    Status = "Chưa đăng ký"
                };

                dbContext.Attendees.Add(attendee);
            }

            await dbContext.SaveChangesAsync();
            return Results.Ok(new
            {
                message = "Tạo người dùng mới thành công."
            });
        });

        // change password
        endpoints.MapPut("api/user/changepassword", async (ChangePasswordDto userChange, AppDbContext dbContext) =>
        {
            User? user = dbContext.Users.Find(userChange.UserId);

            if (user == null)
            {
                return Results.NotFound();
            }

            var oldPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(userChange.OldPassword));

            if (oldPassword != user.Password)
            {
                return Results.Conflict(new
                {
                    message = "Mật khẩu cũ không chính xác"
                });
            }

            var password = Convert.ToBase64String(Encoding.UTF8.GetBytes(userChange.NewPassword));
            user.Password = password;

            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        });
    }
}

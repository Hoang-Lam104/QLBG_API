using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class MeetingEndpoints
{
    public static RouteGroupBuilder MapMeetingEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/meetings");

        group.MapGet("/", (AppDbContext dbContext) =>
        {
            return Results.Ok(dbContext.Meetings);
        });

        group.MapGet("/{id}", (int id, AppDbContext dbContext) =>
        {
            Meeting? meeting = dbContext.Meetings.Find(id);

            return meeting is null ? Results.NotFound() : Results.Ok(meeting);
        });

        group.MapPost("/", async (CreateMeetingDtos newMeeting, AppDbContext dbContext) =>
        {
            Meeting meeting = new()
            {
                Title = newMeeting.Title,
                Date = newMeeting.Date
            };

            dbContext.Meetings.Add(meeting);
            await dbContext.SaveChangesAsync();

            var users = await dbContext.Users.ToListAsync();
            var admin = users.Find(user => user.Id == 1);
            users.Remove(admin!);

            foreach (var user in users)
            {
                var attendee = new Attendee
                {
                    UserId = user.Id,
                    MeetingId = meeting.Id,
                    RoomId = 1,
                    IsMeeting = true
                };

                dbContext.Attendees.Add(attendee);
            }

            await dbContext.SaveChangesAsync();
            return Results.Ok(dbContext.Meetings);
        });

        group.MapDelete("/{id}", async (int id, AppDbContext dbContext) =>
        {
            var meeting = dbContext.Meetings.Find(id);
            if (meeting == null)
            {
                return Results.NotFound();
            }

            dbContext.Meetings.Remove(meeting);

            var attendees = await dbContext.Attendees.ToListAsync();

            foreach (var attendee in attendees)
            {
                if (attendee.MeetingId == id)
                {
                    dbContext.Attendees.Remove(attendee);
                }
            }

            await dbContext.SaveChangesAsync();

            return Results.Ok(dbContext.Meetings);
        });

        group.MapGet("/{id}/attendees", async (int id, AppDbContext dbContext) =>
        {
            var meeting = dbContext.Meetings.Find(id);

            if (meeting == null) return Results.NotFound();

            var attendees = await dbContext.Attendees.ToListAsync();
            var list = attendees.Where(attendee => attendee.MeetingId == id);

            var users = new List<AttendeeDto>();

            foreach (var item in list)
            {
                var userEntity = dbContext.Users.Find(item.UserId);
                var userDto = new AttendeeDto
                (
                    Id: userEntity!.Id,
                    Fullname: userEntity!.Fullname,
                    DepartmentId: userEntity.DepartmentId,
                    Position: userEntity.Position,
                    RoomId: item.RoomId,
                    IsMeeting: item.IsMeeting
                );

                users.Add(userDto);
            }

            return Results.Ok(users);
        });

        return group;
    }
}

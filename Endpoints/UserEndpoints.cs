using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class UserEndpoints
{
    public static RouteGroupBuilder MapUserEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/user");

        group.MapGet("/{id}/info", (int id, AppDbContext dbContext) =>
        {
            User? user = dbContext.Users.Find(id);

            return user is null ? Results.NotFound() : Results.Ok(user);
        });

        group.MapGet("/{id}/meetings", async (int id, AppDbContext dbContext) =>
        {
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
                    Reason: item.Reason!,
                    IsMeeting: item.IsMeeting,
                    Date: meetingEntity.Date
                );

                meetings.Add(meetingDto);
            }

            return Results.Ok(meetings);
        });

        group.MapPut("/attend", async (UpdateAttendeeDto updateAttendee, AppDbContext dbContext) =>
        {
            Attendee? attendee = await dbContext.Attendees.FirstOrDefaultAsync(a =>
                a.UserId == updateAttendee.UserId && a.MeetingId == updateAttendee.MeetingId
            );

            if (attendee is null) return Results.NotFound();

            attendee.RoomId = updateAttendee.RoomId;
            attendee.IsMeeting = updateAttendee.IsMeeting;
            attendee.Reason = updateAttendee.Reason.Length > 0 ? updateAttendee.Reason : null;

            dbContext.Entry(attendee).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            var attendees = await dbContext.Attendees.ToListAsync();
            var list = attendees.Where(attendee => attendee.UserId == updateAttendee.UserId);

            var meetings = new List<AttenMeetingDto>();

            foreach (var item in list)
            {
                var meetingEntity = dbContext.Meetings.Find(item.MeetingId);
                var meetingDto = new AttenMeetingDto
                (
                    Id: meetingEntity!.Id,
                    Title: meetingEntity.Title,
                    RoomId: item.RoomId,
                    Reason: item.Reason!,
                    IsMeeting: item.IsMeeting,
                    Date: meetingEntity.Date
                );

                meetings.Add(meetingDto);
            }

            return Results.Ok(meetings);
        });

        group.MapPut("/{id}/info", async (int id, UpdateUserDto updateUser, AppDbContext dbContext) =>
        {
            User? user = dbContext.Users.Find(id);

            if (user is null) return Results.NotFound();

            user.Fullname = updateUser.Fullname;
            user.DepartmentId = updateUser.DepartmentId;
            user.Position = updateUser.Position;

            dbContext.Entry(user).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.Ok(user);
        });

        return group;
    }
}

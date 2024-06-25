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
        }).RequireAuthorization();

        group.MapGet("/{id}/meetings", async (
            int id,
            int pageIndex,
            int numberInPage,
            AppDbContext dbContext,
            DateTime? startTime = null,
            DateTime? endTime = null) =>
        {
            startTime ??= DateTime.MinValue;
            endTime ??= DateTime.MaxValue;

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
                    Status: item.Status!,
                    Date: meetingEntity.Date
                );

                if (meetingDto.Date >= startTime && meetingDto.Date <= endTime)
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

        group.MapPut("/attend", async (UpdateAttendeeDto updateAttendee, AppDbContext dbContext) =>
        {
            Attendee? attendee = await dbContext.Attendees.FirstOrDefaultAsync(a =>
                a.UserId == updateAttendee.UserId && a.MeetingId == updateAttendee.MeetingId
            );

            if (attendee is null) return Results.NotFound();

            attendee.RoomId = updateAttendee.RoomId;
            attendee.Status = updateAttendee.Status;
            attendee.Reason = updateAttendee.Reason.Length > 0 ? updateAttendee.Reason : null;

            dbContext.Entry(attendee).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization();

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
        }).RequireAuthorization();

        return group;
    }
}

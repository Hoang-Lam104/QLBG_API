using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class MeetingEndpoints
{
    public static void MapMeetingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        // get list meeting (role: admin)
        endpoints.MapGet("/api/meetings", (
            int pageIndex,
            int numberInPage,
            AppDbContext dbContext,
            DateTime? startTime,
            DateTime? endTime) =>
        {
            startTime ??= DateTime.MinValue;
            endTime ??= DateTime.MaxValue;

            var total = dbContext.Meetings.Count();
            var meetings = dbContext.Meetings
                .Where(m => m.StartTime >= startTime && m.EndTime <= endTime)
                .OrderByDescending(m => m.Id)
                .Skip((pageIndex - 1) * numberInPage)
                .Take(numberInPage)
                .ToList();

            MeetingPagi meetingPagi = new()
            {
                Meetings = meetings,
                Total = total
            };

            return Results.Ok(meetingPagi);
        }).RequireAuthorization();

        // get detail meeting
        endpoints.MapGet("api/meetings/{id}", (int id, AppDbContext dbContext) =>
        {
            Meeting? meeting = dbContext.Meetings.Find(id);

            if (meeting is null || !meeting.IsActive)
            {
                return Results.NotFound();
            }
            else
            {
                return Results.Ok(meeting);
            }
        }).RequireAuthorization();

        // create new meeting
        endpoints.MapPost("api/meetings/", async (CreateMeetingDtos newMeeting, AppDbContext dbContext) =>
        {
            Meeting meeting = new()
            {
                Title = newMeeting.Title,
                StartTime = newMeeting.StartTime,
                EndTime = newMeeting.EndTime,
                IsActive = true
            };

            dbContext.Meetings.Add(meeting);
            await dbContext.SaveChangesAsync();

            var users = await dbContext.Users.Where(u => u.IsActive).ToListAsync();
            var admin = users.Find(user => user.Id == 1);
            users.Remove(admin!);

            foreach (var user in users)
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
            return Results.NoContent();
        });

        // toggle active meeting
        endpoints.MapPut("api/meetings/active/{id}", async (int id, AppDbContext dbContext) =>
        {
            var meeting = dbContext.Meetings.Find(id);
            if (meeting == null)
            {
                return Results.NotFound();
            }

            meeting.IsActive = !meeting.IsActive;

            dbContext.Entry(meeting).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            return Results.NoContent();
        }).RequireAuthorization();

        // get list attendees of meeting by id
        endpoints.MapGet("api/meetings/{id}/attendees", async (
            int id,
            int pageIndex,
            int numberInPage,
            AppDbContext dbContext,
            string? search,
            string? position,
            int? departmentId,
            int? roomId,
            string? status,
            int? reasonId) =>
        {
            search ??= null;
            position ??= null;
            departmentId ??= 0;
            roomId ??= 0;
            status ??= null;
            reasonId ??= 0;

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
                    Status: item.Status!,
                    ReasonId: item.ReasonId,
                    AnotherReason: item.AnotherReason
                );

                if (
                    (search == null || userDto.Fullname.ToLower().Contains(search))
                    && (departmentId == 0 || userDto.DepartmentId == departmentId)
                    && (roomId == 0 || userDto.RoomId == roomId)
                    && (position == null || userDto.Position == position)
                    && (reasonId == 0 || reasonId == userDto.ReasonId)
                    && (status?.Length <= 0 || status == userDto.Status)
                )
                {
                    users.Add(userDto);
                }
            }

            AttendeePaging attendeePaging = new()
            {
                Attendees = users.OrderBy(u => u.Id).Skip((pageIndex - 1) * numberInPage).Take(numberInPage).ToList(),
                Total = users.Count()
            };

            return Results.Ok(attendeePaging);
        }).RequireAuthorization();
    }
}

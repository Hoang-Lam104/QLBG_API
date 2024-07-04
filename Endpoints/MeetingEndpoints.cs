﻿using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;

namespace QLGB.API.Endpoints;

public static class MeetingEndpoints
{
    public static RouteGroupBuilder MapMeetingEndpoint(this WebApplication app)
    {
        var group = app.MapGroup("api/meetings");

        // get list meeting (role: admin)
        group.MapGet("/", (
            int pageIndex,
            int numberInPage,
            AppDbContext dbContext,
            DateTime? startTime = null,
            DateTime? endTime = null) =>
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
        group.MapGet("/{id}", (int id, AppDbContext dbContext) =>
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
        group.MapPost("/", async (CreateMeetingDtos newMeeting, AppDbContext dbContext) =>
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
        group.MapPut("/active/{id}", async (int id, AppDbContext dbContext) =>
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
        group.MapGet("/{id}/attendees", async (
            int id,
            int pageIndex,
            int numberInPage,
            AppDbContext dbContext,
            string? search = "",
            string? position = null,
            int? departmentId = 0,
            int? roomId = 0,
            string? status = "",
            int? reasonId = null) =>
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
                    Status: item.Status!,
                    ReasonId: item.ReasonId,
                    AnotherReason: item.AnotherReason
                );

                if (
                    userDto.Fullname.ToLower().Contains(search)
                    && (departmentId == 0 || userDto.DepartmentId == departmentId)
                    && (roomId == 0 || userDto.RoomId == roomId)
                    && (position == null || userDto.Position == position)
                    && (reasonId == null || reasonId == userDto.ReasonId)
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

        return group;
    }
}

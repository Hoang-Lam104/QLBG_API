using Microsoft.EntityFrameworkCore;
using QLGB.API.Data;
using QLGB.API.Dtos;
using QLGB.API.Models;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.IO;

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

        endpoints.MapGet("api/meetings/{id}/export", async (int id, AppDbContext dbContext) =>
        {
            var meeting = await dbContext.Meetings.FindAsync(id);

            if (meeting == null) return Results.NotFound();

            var attendees = await dbContext.Attendees
                .Where(a => a.MeetingId == id)
                .ToListAsync();

            var users = new List<AttendeeDto>();
            var departments = dbContext.Departments;
            var reasons = dbContext.Reasons;
            var rooms = dbContext.Rooms;

            foreach (var item in attendees)
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

                {
                    users.Add(userDto);
                }
            }

            // Tạo workbook mới
            IWorkbook workbook = new XSSFWorkbook();

            // Tạo sheet mới
            ISheet sheet = workbook.CreateSheet("Họp giao ban");

            // Tạo hàng đầu tiên và gán giá trị vào các ô
            IRow row1 = sheet.CreateRow(0);
            row1.CreateCell(0).SetCellValue("STT");
            row1.CreateCell(1).SetCellValue("Họ tên");
            row1.CreateCell(2).SetCellValue("Chức vụ");
            row1.CreateCell(3).SetCellValue("Phòng ban");
            row1.CreateCell(4).SetCellValue("Tình trạng");
            row1.CreateCell(5).SetCellValue("Hội trường");
            row1.CreateCell(6).SetCellValue("Lý do vắng");

            for (var i = 0; i < users.Count(); i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                row.CreateCell(0).SetCellValue(i + 1);
                row.CreateCell(1).SetCellValue(users[i].Fullname);
                row.CreateCell(2).SetCellValue(users[i].Position);
                row.CreateCell(3).SetCellValue(departments.Find(users[i].DepartmentId)?.Name);
                row.CreateCell(4).SetCellValue(users[i].Status);
                row.CreateCell(5).SetCellValue(rooms.Find(users[i].RoomId)?.Name);
                row.CreateCell(6).SetCellValue(reasons.Find(users[i].ReasonId)?.Name);
            };

            // var memoryStream = new MemoryStream();
            // workbook.Write(memoryStream);
            // memoryStream.Position = 0;

            // // Đóng workbook
            // workbook.Close();

            // return Results.File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "example.xlsx");

            // Đường dẫn tới file xuất
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            string filePath = Path.Combine(downloadsPath, "example.xlsx");

            // Ghi workbook vào file
            using (FileStream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                workbook.Write(stream);
            }

            // Đóng workbook
            workbook.Close();

            return Results.Ok(new
            {
                message = "Tải xuống thành công"
            });
        });
    }
}

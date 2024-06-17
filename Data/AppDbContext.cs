namespace QLGB.API.Data;

using Microsoft.EntityFrameworkCore;
using QLGB.API.Models;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
    public DbSet<Department> Departments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendee>()
            .HasOne(a => a.User)
            .WithMany(u => u.Attendees)
            .HasForeignKey(a => a.UserId);

        modelBuilder.Entity<Attendee>()
            .HasOne(a => a.Meeting)
            .WithMany(m => m.Attendees)
            .HasForeignKey(a => a.MeetingId);

        modelBuilder.Entity<Attendee>()
            .HasOne(a => a.Room)
            .WithMany(r => r.Attendees)
            .HasForeignKey(a => a.RoomId);

        modelBuilder.Entity<Room>().HasData(
            new { Id = 1, Name = "Hội trường 1 CS1" },
            new { Id = 2, Name = "Hội trường 2 CS1" },
            new { Id = 3, Name = "Hội trường 3 CS1" },
            new { Id = 4, Name = "Hội trường CS2" }
        );

        modelBuilder.Entity<Department>().HasData(
            new { Id = 1, Name = "Khoa Bệnh nhiệt đới" },
            new { Id = 2, Name = "Khoa phẫu thuật, gây mê - Hồi sức, cấp cứu" },
            new { Id = 3, Name = "Khoa y học cổ truyền - Phục hồi chức năng" },
            new { Id = 4, Name = "Khoa ngoại" },
            new { Id = 5, Name = "Khoa mắt" },
            new { Id = 6, Name = "Ban lãnh đạo" }
        );

        modelBuilder.Entity<User>().HasData(
            new
            {
                Id = 1,
                Username = "admin",
                Password = "123",
                Fullname = "Admin",
                DepartmentId = 6,
                Position = "Admin"
            },
            new
            {
                Id = 2,
                Username = "ANV",
                Password = "123",
                Fullname = "Nguyễn Văn A",
                DepartmentId = 1,
                Position = "Trưởng khoa",
            },
            new
            {
                Id = 3,
                Username = "BLT",
                Password = "123",
                Fullname = "Lê Thị B",
                DepartmentId = 2,
                Position = "Điều dưỡng trưởng",
            }
        );
    }
}


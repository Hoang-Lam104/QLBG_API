namespace QLGB.API.Data;

using Microsoft.EntityFrameworkCore;
using QLGB.API.Models;

public class AppDbContext : DbContext
// (DbContextOptions<AppDbContext> options) : base(options)
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Log> Log { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Reason> Reasons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Log>()
            .HasOne(l => l.User)
            .WithMany(u => u.Logs)
            .HasForeignKey(l => l.UserId);

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

        modelBuilder.Entity<Attendee>()
            .HasOne(a => a.Reason)
            .WithMany(r => r.Attendees)
            .HasForeignKey(a => a.ReasonId);

        modelBuilder.Entity<Room>().HasData(
            new
            {
                Id = 1,
                Name = "Hội trường 1 CS1",
                IsActive = true,
            },
            new
            {
                Id = 2,
                Name = "Hội trường 2 CS1",
                IsActive = true,
            },
            new
            {
                Id = 3,
                Name = "Hội trường 3 CS1",
                IsActive = true,
            },
            new
            {
                Id = 4,
                Name = "Hội trường CS2",
                IsActive = true,
            }
        );

        modelBuilder.Entity<Reason>().HasData(
            new
            {
                ReasonId = 1,
                Title = "Khác",
                IsActive = true,
            },
            new
            {
                ReasonId = 2,
                Title = "Nghỉ bù trực",
                IsActive = true,
            },
            new
            {
                ReasonId = 3,
                Title = "Khám bệnh tại phòng khám",
                IsActive = true,
            },
            new
            {
                ReasonId = 4,
                Title = "Ở lại khoa làm việc",
                IsActive = true,
            }
        );

        modelBuilder.Entity<Department>().HasData(
            new { Id = 1, Name = "Ban lãnh đạo" },
            new { Id = 2, Name = "Khoa phẫu thuật, gây mê - Hồi sức, cấp cứu" },
            new { Id = 3, Name = "Khoa y học cổ truyền - Phục hồi chức năng" },
            new { Id = 4, Name = "Khoa ngoại" },
            new { Id = 5, Name = "Khoa mắt" },
            new { Id = 6, Name = "Khoa Bệnh nhiệt đới" },
            new { Id = 7, Name = "Khoa chẩn đoán hình ảnh" },
            new { Id = 8, Name = "Khoa nhi" },
            new { Id = 9, Name = "Khoa HSTC - Chống độc - Thận nhân tạo" },
            new { Id = 10, Name = "Khoa Nội tổng hợp" },
            new { Id = 11, Name = "Khoa Nội Tim Mạch" },
            new { Id = 12, Name = "Khoa khám bệnh" },
            new { Id = 13, Name = "Khoa Răng Hàm Mặt" },
            new { Id = 14, Name = "Khoa Tai Mũi Họng" },
            new { Id = 15, Name = "Khoa Phụ Sản" },
            new { Id = 16, Name = "Phòng Công tác xã hội" },
            new { Id = 17, Name = "Phòng Điều dưỡng" },
            new { Id = 18, Name = "Phòng Kế hoạch tổng hợp" },
            new { Id = 19, Name = "Phòng Quản lý chất lượng - Đào tạo và Chỉ đạo tuyến" },
            new { Id = 20, Name = "Phòng Tài chính kế toán" },
            new { Id = 21, Name = "Phòng Tổ chức hành chính" },
            new { Id = 22, Name = "Phòng Vật tư kỹ thuật - Trang thiết bị và Công nghệ thông tin" }
        );

        modelBuilder.Entity<User>().HasData(
            new
            {
                Id = 1,
                Username = "admin",
                Password = "MTIz",
                Fullname = "Admin",
                DepartmentId = 1,
                Position = "Admin",
                IsActive = true,
            }
        );
    }
}


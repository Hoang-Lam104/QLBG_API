namespace QLGB.API.Data;

using Microsoft.EntityFrameworkCore;
using QLGB.API.Models;

public class AppDbContext : DbContext
{
    public DbSet<Log> Log { get; set; }
    public DbSet<Meeting> Meetings { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Attendee> Attendees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Reason> Reasons { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Log = Set<Log>();
        Meetings = Set<Meeting>();
        Rooms = Set<Room>();
        Users = Set<User>();
        Attendees = Set<Attendee>();
        Departments = Set<Department>();
        Reasons = Set<Reason>();
    }
}


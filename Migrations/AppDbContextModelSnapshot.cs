﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QLGB.API.Data;

#nullable disable

namespace QLGB.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QLGB.API.Models.Attendee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AnotherReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MeetingId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("MeetingTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ReasonId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("RegisterTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("RoomId")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("ReasonId");

                    b.HasIndex("RoomId");

                    b.HasIndex("UserId");

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("QLGB.API.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Ban lãnh đạo"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Khoa phẫu thuật, gây mê - Hồi sức, cấp cứu"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Khoa y học cổ truyền - Phục hồi chức năng"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Khoa ngoại"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Khoa mắt"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Khoa Bệnh nhiệt đới"
                        });
                });

            modelBuilder.Entity("QLGB.API.Models.Log", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("DeviceInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogEvent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LogTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("QLGB.API.Models.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("QLGB.API.Models.Reason", b =>
                {
                    b.Property<int>("ReasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ReasonId"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ReasonId");

                    b.ToTable("Reasons");

                    b.HasData(
                        new
                        {
                            ReasonId = 1,
                            IsActive = true,
                            Title = "Khác"
                        },
                        new
                        {
                            ReasonId = 2,
                            IsActive = true,
                            Title = "Nghỉ bù trực"
                        },
                        new
                        {
                            ReasonId = 3,
                            IsActive = true,
                            Title = "Khám bệnh tại phòng khám"
                        },
                        new
                        {
                            ReasonId = 4,
                            IsActive = true,
                            Title = "Ở lại khoa làm việc"
                        });
                });

            modelBuilder.Entity("QLGB.API.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsActive = true,
                            Name = "Hội trường 1 CS1"
                        },
                        new
                        {
                            Id = 2,
                            IsActive = true,
                            Name = "Hội trường 2 CS1"
                        },
                        new
                        {
                            Id = 3,
                            IsActive = true,
                            Name = "Hội trường 3 CS1"
                        },
                        new
                        {
                            Id = 4,
                            IsActive = true,
                            Name = "Hội trường CS2"
                        });
                });

            modelBuilder.Entity("QLGB.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepartmentId")
                        .HasColumnType("int");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DepartmentId = 1,
                            Fullname = "Admin",
                            IsActive = true,
                            Password = "123",
                            Position = "Admin",
                            Username = "admin"
                        },
                        new
                        {
                            Id = 2,
                            DepartmentId = 2,
                            Fullname = "Nguyễn Văn A",
                            IsActive = true,
                            Password = "123",
                            Position = "Trưởng khoa",
                            Username = "ANV"
                        },
                        new
                        {
                            Id = 3,
                            DepartmentId = 3,
                            Fullname = "Lê Thị B",
                            IsActive = true,
                            Password = "123",
                            Position = "Điều dưỡng trưởng",
                            Username = "BLT"
                        });
                });

            modelBuilder.Entity("QLGB.API.Models.Attendee", b =>
                {
                    b.HasOne("QLGB.API.Models.Meeting", "Meeting")
                        .WithMany("Attendees")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLGB.API.Models.Reason", "Reason")
                        .WithMany("Attendees")
                        .HasForeignKey("ReasonId");

                    b.HasOne("QLGB.API.Models.Room", "Room")
                        .WithMany("Attendees")
                        .HasForeignKey("RoomId");

                    b.HasOne("QLGB.API.Models.User", "User")
                        .WithMany("Attendees")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");

                    b.Navigation("Reason");

                    b.Navigation("Room");

                    b.Navigation("User");
                });

            modelBuilder.Entity("QLGB.API.Models.Log", b =>
                {
                    b.HasOne("QLGB.API.Models.User", "User")
                        .WithMany("Logs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("QLGB.API.Models.User", b =>
                {
                    b.HasOne("QLGB.API.Models.Department", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("QLGB.API.Models.Meeting", b =>
                {
                    b.Navigation("Attendees");
                });

            modelBuilder.Entity("QLGB.API.Models.Reason", b =>
                {
                    b.Navigation("Attendees");
                });

            modelBuilder.Entity("QLGB.API.Models.Room", b =>
                {
                    b.Navigation("Attendees");
                });

            modelBuilder.Entity("QLGB.API.Models.User", b =>
                {
                    b.Navigation("Attendees");

                    b.Navigation("Logs");
                });
#pragma warning restore 612, 618
        }
    }
}

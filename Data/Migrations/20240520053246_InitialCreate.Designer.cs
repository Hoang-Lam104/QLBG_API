﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QLGB.API.Data;

#nullable disable

namespace QLGB.API.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240520053246_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.5");

            modelBuilder.Entity("QLGB.API.Models.Attendee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsMeeting")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MeetingId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Reason")
                        .HasColumnType("TEXT");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MeetingId");

                    b.HasIndex("RoomId");

                    b.HasIndex("UserId");

                    b.ToTable("Attendees");
                });

            modelBuilder.Entity("QLGB.API.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Khoa Bệnh nhiệt đới"
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
                            Name = "Ban lãnh đạo"
                        });
                });

            modelBuilder.Entity("QLGB.API.Models.Meeting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("QLGB.API.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Rooms");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Hội trường 1"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Hội trường 2"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Hội trường CS2"
                        });
                });

            modelBuilder.Entity("QLGB.API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DepartmentId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Fullname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DepartmentId = 6,
                            Fullname = "Admin",
                            Position = "Admin"
                        },
                        new
                        {
                            Id = 2,
                            DepartmentId = 1,
                            Fullname = "Nguyễn Văn A",
                            Position = "Trưởng khoa"
                        },
                        new
                        {
                            Id = 3,
                            DepartmentId = 2,
                            Fullname = "Lê Thị B",
                            Position = "Điều dưỡng trưởng"
                        });
                });

            modelBuilder.Entity("QLGB.API.Models.Attendee", b =>
                {
                    b.HasOne("QLGB.API.Models.Meeting", "Meeting")
                        .WithMany("Attendees")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLGB.API.Models.Room", "Room")
                        .WithMany("Attendees")
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("QLGB.API.Models.User", "User")
                        .WithMany("Attendees")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Meeting");

                    b.Navigation("Room");

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

            modelBuilder.Entity("QLGB.API.Models.Room", b =>
                {
                    b.Navigation("Attendees");
                });

            modelBuilder.Entity("QLGB.API.Models.User", b =>
                {
                    b.Navigation("Attendees");
                });
#pragma warning restore 612, 618
        }
    }
}

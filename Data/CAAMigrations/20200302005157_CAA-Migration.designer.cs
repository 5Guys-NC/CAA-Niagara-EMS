﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CAA_Event_Management.Data;

namespace CAA_Event_Management.Migrations
{
    [DbContext(typeof(CAAContext))]
    [Migration("20200302005157_CAA-Migration")]
    partial class CAAMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.6");

            modelBuilder.Entity("CAA_Event_Management.Models.Answer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool?>("IsCorrect");

                    b.Property<string>("Keywords");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(75);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Phrase")
                        .HasMaxLength(50);

                    b.Property<int>("QuestionID");

                    b.HasKey("ID");

                    b.HasIndex("QuestionID");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.AttendanceItem", b =>
                {
                    b.Property<string>("MemberAttendanceID");

                    b.Property<string>("EventItemID");

                    b.Property<string>("Answer")
                        .HasMaxLength(50);

                    b.HasKey("MemberAttendanceID", "EventItemID");

                    b.HasIndex("EventItemID");

                    b.ToTable("AttendanceItems");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.AttendanceTracking", b =>
                {
                    b.Property<string>("MemberAttendanceID")
                        .HasMaxLength(36);

                    b.Property<DateTime>("ArrivalTime");

                    b.Property<string>("EventID")
                        .IsRequired();

                    b.Property<bool>("ExternalData");

                    b.Property<string>("FirstName")
                        .HasMaxLength(25);

                    b.Property<string>("IsMember")
                        .HasMaxLength(20);

                    b.Property<string>("LastName")
                        .HasMaxLength(25);

                    b.Property<string>("MemberNo")
                        .HasMaxLength(16);

                    b.Property<string>("PhoneNo");

                    b.HasKey("MemberAttendanceID");

                    b.HasIndex("EventID");

                    b.ToTable("AttendanceTrackings");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Event", b =>
                {
                    b.Property<string>("EventID")
                        .HasMaxLength(36);

                    b.Property<string>("AbrevEventname")
                        .HasMaxLength(20);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(100);

                    b.Property<DateTime?>("EventEnd");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime?>("EventStart")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Keywords");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(75);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<bool>("MembersOnly");

                    b.Property<int?>("QuizID");

                    b.HasKey("EventID");

                    b.HasIndex("QuizID")
                        .IsUnique();

                    b.ToTable("Events");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.EventGameUserAnswer", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(36);

                    b.Property<string>("EventID")
                        .IsRequired();

                    b.Property<int>("QuestionID");

                    b.Property<bool>("answerWasCorrect");

                    b.HasKey("ID");

                    b.HasIndex("EventID");

                    b.HasIndex("QuestionID");

                    b.ToTable("EventGameUserAnswers");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.EventItem", b =>
                {
                    b.Property<string>("EventItemID")
                        .HasMaxLength(36);

                    b.Property<string>("EventID")
                        .IsRequired();

                    b.Property<string>("ItemID")
                        .IsRequired();

                    b.HasKey("EventItemID");

                    b.HasIndex("EventID");

                    b.HasIndex("ItemID");

                    b.ToTable("EventItems");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Game", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("Keywords");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(75);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Item", b =>
                {
                    b.Property<string>("ItemID")
                        .HasMaxLength(36);

                    b.Property<bool>("IsDeleted");

                    b.Property<int>("ItemCount");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasMaxLength(75);

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.HasKey("ItemID");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Question", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CorrectFeedback");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(250);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<int>("GameID");

                    b.Property<string>("Keywords");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(75);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Phrase");

                    b.Property<int>("TotalFeedback");

                    b.HasKey("ID");

                    b.HasIndex("GameID");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Users", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("FirstName")
                        .HasMaxLength(25);

                    b.Property<string>("LastName")
                        .HasMaxLength(25);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .IsRequired();

                    b.Property<bool>("isAdmin");

                    b.HasKey("ID");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Answer", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionID");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.AttendanceItem", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.EventItem", "EventItem")
                        .WithMany("AttendanceItems")
                        .HasForeignKey("EventItemID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CAA_Event_Management.Models.AttendanceTracking", "AttendanceTracking")
                        .WithMany("AttendanceItems")
                        .HasForeignKey("MemberAttendanceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CAA_Event_Management.Models.AttendanceTracking", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Event", "Event")
                        .WithMany("AttendanceTrackings")
                        .HasForeignKey("EventID");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Event", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Game", "Quiz")
                        .WithOne("Event")
                        .HasForeignKey("CAA_Event_Management.Models.Event", "QuizID");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.EventGameUserAnswer", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CAA_Event_Management.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CAA_Event_Management.Models.EventItem", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Event", "Event")
                        .WithMany("EventItems")
                        .HasForeignKey("EventID");

                    b.HasOne("CAA_Event_Management.Models.Item", "Item")
                        .WithMany("EventItems")
                        .HasForeignKey("ItemID");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Question", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Game", "Game")
                        .WithMany("Questions")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

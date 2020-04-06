using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CAA_Event_Management.Data;

namespace CAA_Event_Management.Data.Migrations
{
    [DbContext(typeof(CAAContext))]
    [Migration("20200406001403_CAAMigration")]
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

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Text")
                        .HasMaxLength(50);

                    b.Property<int>("TimesUsed");

                    b.HasKey("ID");

                    b.ToTable("Answers");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.AnswerPicture", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnswerID");

                    b.Property<int>("PictureID");

                    b.HasKey("ID");

                    b.HasIndex("AnswerID");

                    b.HasIndex("PictureID");

                    b.ToTable("AnswerPictures");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.AnswerTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AnswerID");

                    b.Property<int>("TagID");

                    b.HasKey("ID");

                    b.HasIndex("AnswerID");

                    b.HasIndex("TagID");

                    b.ToTable("AnswerTags");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.AttendanceItem", b =>
                {
                    b.Property<string>("MemberAttendanceID")
                        .HasMaxLength(36);

                    b.Property<string>("EventItemID")
                        .HasMaxLength(36);

                    b.Property<string>("Answer")
                        .HasMaxLength(100);

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
                        .IsRequired()
                        .HasMaxLength(36);

                    b.Property<bool>("ExternalData");

                    b.Property<string>("FirstName")
                        .HasMaxLength(50);

                    b.Property<int?>("IsAnEventWinner");

                    b.Property<string>("IsMember")
                        .HasMaxLength(20);

                    b.Property<string>("LastName")
                        .HasMaxLength(75);

                    b.Property<string>("MemberNo")
                        .HasMaxLength(16);

                    b.Property<string>("PhoneNo")
                        .HasMaxLength(50);

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
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("DisplayName")
                        .HasMaxLength(155);

                    b.Property<DateTime?>("EventEnd");

                    b.Property<string>("EventName")
                        .IsRequired()
                        .HasMaxLength(155);

                    b.Property<DateTime?>("EventStart")
                        .IsRequired();

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("Keywords");

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<bool>("MembersOnly");

                    b.Property<int?>("QuizID");

                    b.HasKey("EventID");

                    b.HasIndex("QuizID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.EventGameUserAnswer", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(36);

                    b.Property<string>("AttendantID")
                        .HasMaxLength(36);

                    b.Property<string>("EventID")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.Property<int>("QuestionID");

                    b.Property<int?>("answerID");

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

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("EventID")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.Property<string>("ItemID")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.HasKey("EventItemID");

                    b.HasIndex("EventID");

                    b.HasIndex("ItemID");

                    b.ToTable("EventItems");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Game", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.GameModel", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AnswerText");

                    b.Property<int>("GameID");

                    b.Property<string>("ImageIDs");

                    b.Property<string>("OptionsText");

                    b.Property<string>("QuestionText");

                    b.HasKey("ID");

                    b.HasIndex("GameID");

                    b.ToTable("GameModels");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.GameTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameID");

                    b.Property<int>("TagID");

                    b.HasKey("ID");

                    b.HasIndex("GameID");

                    b.HasIndex("TagID");

                    b.ToTable("GameTags");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Item", b =>
                {
                    b.Property<string>("ItemID")
                        .HasMaxLength(36);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<bool?>("IsDeleted");

                    b.Property<int?>("ItemCount");

                    b.Property<string>("ItemName")
                        .IsRequired()
                        .HasMaxLength(150);

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("ValueType")
                        .IsRequired()
                        .HasMaxLength(25);

                    b.HasKey("ItemID");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.ModelAuditLine", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(36);

                    b.Property<string>("AuditorName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("ChangedFieldValues")
                        .IsRequired()
                        .HasMaxLength(5000);

                    b.Property<string>("DateTimeOfChange")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.Property<string>("NewObjectInfo")
                        .IsRequired()
                        .HasMaxLength(5000);

                    b.Property<string>("ObjectID")
                        .IsRequired()
                        .HasMaxLength(36);

                    b.Property<string>("ObjectTable")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TypeOfChange")
                        .IsRequired()
                        .HasMaxLength(30);

                    b.HasKey("ID");

                    b.ToTable("ModelAuditLines");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Picture", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<byte[]>("Image");

                    b.Property<string>("ImageFileName");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.HasKey("ID");

                    b.ToTable("Pictures");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Question", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Text");

                    b.Property<int>("TimesUsed");

                    b.HasKey("ID");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.QuestionTag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("QuestionID");

                    b.Property<int>("TagID");

                    b.HasKey("ID");

                    b.HasIndex("QuestionID");

                    b.HasIndex("TagID");

                    b.ToTable("QuestionTags");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.Tag", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("LastModifiedBy");

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.UserAccount", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(36);

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("CreatedDate");

                    b.Property<string>("FirstName")
                        .HasMaxLength(25);

                    b.Property<string>("LastModifiedBy")
                        .HasMaxLength(256);

                    b.Property<DateTime?>("LastModifiedDate");

                    b.Property<string>("LastName")
                        .HasMaxLength(25);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("isAdmin");

                    b.HasKey("ID");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("UserAccounts");
                });

            modelBuilder.Entity("CAA_Event_Management.Models.AnswerPicture", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Answer", "Answer")
                        .WithMany("AnswerPictures")
                        .HasForeignKey("AnswerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CAA_Event_Management.Models.Picture", "Picture")
                        .WithMany("AnswerPictures")
                        .HasForeignKey("PictureID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CAA_Event_Management.Models.AnswerTag", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Answer", "Answer")
                        .WithMany()
                        .HasForeignKey("AnswerID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CAA_Event_Management.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagID")
                        .OnDelete(DeleteBehavior.Cascade);
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
                        .WithMany()
                        .HasForeignKey("QuizID");
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

            modelBuilder.Entity("CAA_Event_Management.Models.GameModel", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Game", "Game")
                        .WithMany("GameModels")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CAA_Event_Management.Models.GameTag", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CAA_Event_Management.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CAA_Event_Management.Models.QuestionTag", b =>
                {
                    b.HasOne("CAA_Event_Management.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CAA_Event_Management.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}

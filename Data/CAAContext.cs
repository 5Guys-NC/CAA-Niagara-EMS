﻿using CAA_Event_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
/******************************
*  Model Created By: Brian Culp
*  Edited by: Jon Yade
*  Edited by: Max Cashmore
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Context page to contain the Database tables restrictions and configuration
    /// </summary>
    public class CAAContext : DbContext
    {
        #region Model DbSets

        public DbSet<Answer> Answers { get; set; }
        public DbSet<AnswerPicture> AnswerPictures { get; set; }
        public DbSet<AnswerTag> AnswerTags { get; set; }
        public DbSet<AttendanceItem> AttendanceItems { get; set; }
        public DbSet<AttendanceTracking> AttendanceTrackings { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventGameUserAnswer> EventGameUserAnswers { get; set; }
        public DbSet<EventItem> EventItems { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameModel> GameModels { get; set; }
        public DbSet<GameTag> GameTags { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionTag> QuestionTags { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Name of Database
            optionsBuilder.UseSqlite("Data Source=CAANiagara.db");
            base.OnConfiguring(optionsBuilder);
        }

        #region Model Restrictions

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Composite Key for AttendanceItem
            modelBuilder.Entity<AttendanceItem>()
                .HasKey(d => new { d.MemberAttendanceID, d.EventItemID });

            //UserAccount Unique Username
            modelBuilder.Entity<UserAccount>().HasIndex(d => d.UserName).IsUnique();

            //EventItem - Item Delete Restrict
            modelBuilder.Entity<EventItem>()
                .HasOne(d => d.Item)
                .WithMany(d => d.EventItems)
                .HasForeignKey(d => d.ItemID)
                .OnDelete(DeleteBehavior.Restrict);

            //EventItem - Event Delete Restrict
            modelBuilder.Entity<EventItem>()
                .HasOne(d => d.Event)
                .WithMany(d => d.EventItems)
                .HasForeignKey(d => d.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            //AttendanceTracking - Event Delete Restrict
            modelBuilder.Entity<AttendanceTracking>()
                .HasOne(d => d.Event)
                .WithMany(d => d.AttendanceTrackings)
                .HasForeignKey(d => d.EventID)
                .OnDelete(DeleteBehavior.Restrict);
        }

        #endregion
    }
}

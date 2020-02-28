using CAA_Event_Management.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public DbSet<Event> Events { get; set; }
        public DbSet<EventItem> EventItems { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<AttendanceTracking> AttendanceTrackings { get; set; }
        public DbSet<AttendanceItem> AttendanceItems { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Users> Users { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=CAANiagara.db");
            base.OnConfiguring(optionsBuilder);
        }

        #region Model Restrictions

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AttendanceItem>()
                .HasKey(d => new { d.MemberAttendanceID, d.EventItemID });

            modelBuilder.Entity<Users>().HasIndex(d => d.UserName).IsUnique();

            modelBuilder.Entity<EventItem>()
                .HasOne(d => d.Item)
                .WithMany(d => d.EventItems)
                .HasForeignKey(d => d.ItemID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<EventItem>()
                .HasOne(d => d.Event)
                .WithMany(d => d.EventItems)
                .HasForeignKey(d => d.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AttendanceTracking>()
                .HasOne(d => d.Event)
                .WithMany(d => d.AttendanceTrackings)
                .HasForeignKey(d => d.EventID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Answer>()
                .HasOne(d => d.Question)
                .WithMany(d => d.Answers)
                .HasForeignKey(d => d.QuestionID)
                .OnDelete(DeleteBehavior.Restrict);
        }

        #endregion
    }
}

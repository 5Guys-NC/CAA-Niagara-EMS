using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/******************************
*  Model Created By: Brian Culp
*  Edited by: Jon Yade
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the EVENTS table in the database
    /// </summary>
    public class Event : Auditable
    {
        public Event()
        {
            this.EventItems = new HashSet<EventItem>();
            this.AttendanceTrackings = new HashSet<AttendanceTracking>();
        }

        #region Table Fields

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string EventID { get; set; } = "0";

        public string DateRange
        {
            get
            {
                string startDate = string.Format("{0:yyyy-MM-dd}", EventStart);
                string endDate = string.Format("{0:yyyy-MM-dd}", EventEnd);
                if (startDate == endDate) return startDate + " (One day)";
                else return startDate + " - " + endDate;
            }
        }

        public string MembersOnlyEvent
        {
            get
            {
                if (MembersOnly == false) return "Open to Public";
                else return "Members Only";
            }
        }

        public string SummaryDisplay
        {
            get
            {
                return DateRange + ", " + MembersOnlyEvent;
            }
        }

        [Display(Name = "Event Name")]
        [Required(ErrorMessage = "Event Name Required")]
        [StringLength(100, ErrorMessage = "Event Name cannot be more then 100 characters")]
        public string EventName { get; set; } = "";

        [Display(Name = "Event Start")]
        [Required(ErrorMessage = "Event Start Required")]
        public DateTime? EventStart { get; set; } = DateTime.Today;

        [Display(Name = "Event End")]
        public DateTime? EventEnd { get; set; } = DateTime.Today;

        [Display(Name = "Members Only?")]
        [Required(ErrorMessage = "MembersOnly Required")]
        public bool MembersOnly { get; set; } = false;

        [Display(Name = "Abbreviated Event Name")]
        [StringLength(20, ErrorMessage = "Abbreviated Event Name cannot be more then 20 characters")]
        public string AbrevEventname { get; set; } = "";

        public int? QuizID { get; set; }
        public virtual Game Quiz { get; set; }

        public string Keywords { get; set; } = "";

        [Display(Name = "Display Name")]
        [StringLength(100, ErrorMessage = "Display Name cannot be more then 100 characters")]
        public string DisplayName { get; set; } = "";

        [Display(Name = "isDeleted?")]
        public bool IsDeleted { get; set; } = false;

        #endregion

        #region Table Connections

        public ICollection<EventItem> EventItems { get; set; }
        public ICollection<AttendanceTracking> AttendanceTrackings { get; set; }


        //https://docs.microsoft.com/en-us/dotnet/api/system.object.equals?view=netframework-4.8 provided this code
        public override bool Equals(object obj)
        { 
              if ((obj == null) || ! this.GetType().Equals(obj.GetType())) 
              {
                 return false;
              }
              else { 
                 Event e = (Event) obj; 
                 return (QuizID == e.QuizID) && (MembersOnly == e.MembersOnly) && (DisplayName == e.DisplayName) && (EventStart == e.EventStart) && (EventEnd == e.EventEnd);
              }   
        }

        #endregion
    }
}

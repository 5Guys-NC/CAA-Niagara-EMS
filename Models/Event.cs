using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/******************************
*  Created By: Brian Culp
*  Edited by: Jon Yade
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the EVENTS table in the database
    /// </summary>
    public class Event
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
                return string.Format("{0:yyyy-MM-dd}",EventStart) + " - " + string.Format("{0:yyyy-MM-dd}", EventEnd);
            }
        }

        public string MembersOnlyEvent
        {
            get
            {
                if (MembersOnly == false) return "Open Event";
                else return "Members Only Event";
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

        [Display(Name = "Created By")]
        [StringLength(250, ErrorMessage = "Created By cannot be more then 250 characters")]
        public string CreatedBy { get; set; } = "CAA Employee";

        [Display(Name = "Date Created")]
        public DateTime? CreatedDate { get; set; } = DateTime.Today;

        [Display(Name = "Display Name")]
        [StringLength(100, ErrorMessage = "Display Name cannot be more then 100 characters")]
        public string DisplayName { get; set; } = "";

        [Display(Name = "Last Modified By")]
        [StringLength(75, ErrorMessage = "Last Modified By cannot be more then 75 characters")]
        public string LastModifiedBy { get; set; } = "CAA Emplyee";

        [Display(Name = "Last Modified Date")]
        public DateTime? LastModifiedDate { get; set; } = DateTime.Today;

        [Display(Name = "isDeleted?")]
        public bool IsDeleted { get; set; } = false;

        #endregion

        #region Table Connections
        
        public ICollection<EventItem> EventItems { get; set; }
        public ICollection<AttendanceTracking> AttendanceTrackings { get; set; }

        #endregion
    }
}

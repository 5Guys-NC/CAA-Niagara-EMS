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
    /// This page is a model to template the ATTENDENCE TRACKING table in the database
    /// </summary>
    public class AttendanceTracking
    {
        public AttendanceTracking()
        {
            this.AttendanceItems = new HashSet<AttendanceItem>();
        }

        #region Table Fields

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string MemberAttendanceID { get; set; } = "0";

        [Display(Name = "Member Number")]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "Member Number must be 16 characters")]
        public string MemberNo { get; set; }

        [Display(Name = "Arrival Time")]
        [Required(ErrorMessage = "Arrival Time Required")]
        public DateTime ArrivalTime { get; set; }

        [Display(Name = "Is Member?")]
        [StringLength(20, ErrorMessage = "Is Member cannot be more then 20 characters")]
        public string IsMember { get; set; }

        [Display(Name = "Phone")]
        [Phone(ErrorMessage = "Phone Number in incorrect format")]
        [StringLength(50)]
        public string PhoneNo { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First Name cannot be more then 25 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(75, ErrorMessage = "Last Name cannot be more then 25 characters")]
        public string LastName { get; set; }

        [Display(Name = "External Data")]
        public bool ExternalData { get; set; }

        [Display(Name = "Person is an event winner")]
        public int? IsAnEventWinner { get; set; } = null;

        #endregion

        #region Table Connections

        [Display(Name = "Event Items ID")]
        [Required(ErrorMessage = "Event Items ID Required")]
        [StringLength(36)]
        public string EventID { get; set; }
        public virtual Event Event { get; set; }
        //public ICollection<Event> Events { get; set; }

        public ICollection<AttendanceItem> AttendanceItems { get; set; }

        #endregion

    }
}
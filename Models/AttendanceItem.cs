using System.ComponentModel.DataAnnotations;
/******************************
*  Model Created By: Brian Culp
*  Edited by: Jon Yade
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for AttendanceItem Table
    /// </summary>
    public class AttendanceItem
    {
        #region Table Fields

        [Display(Name = "Answer")]
        [StringLength(50, ErrorMessage = "Answer cannot be more then 50 characters")]
        public string Answer { get; set; }

        #endregion

        #region Table Connections

        [Display(Name = "Event Items ID")]
        [Required(ErrorMessage = "Event Items ID Required")]
        [StringLength(36)]
        public string EventItemID { get; set; }
        
        public virtual EventItem EventItem { get; set; }


        [Display(Name = "Member Attendance ID")]
        [Required(ErrorMessage = "Member Attendence ID Required")]
        [StringLength(36)]
        public string MemberAttendanceID { get; set; }
        public virtual AttendanceTracking AttendanceTracking { get; set; } 

        #endregion
    }
}

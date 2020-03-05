using System.ComponentModel.DataAnnotations;
/******************************
*  Created By: Brian Culp
*  Edited by: Jon Yade
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the ATTENDENCE ITEM table in the database
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
        public string EventItemID { get; set; }
        public virtual EventItem EventItem { get; set; }


        [Display(Name = "Member Attendance ID")]
        [Required(ErrorMessage = "Member Attendence ID Required")]
        public string MemberAttendanceID { get; set; }
        public virtual AttendanceTracking AttendanceTracking { get; set; }    //Is this going to be a problem

        #endregion
    }
}

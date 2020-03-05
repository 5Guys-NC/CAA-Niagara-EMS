﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/******************************
*  Model Created By: Brian Culp
*  Edited by: Jon Yade
*******************************/
namespace CAA_Event_Management.Models
{
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
        //public ICollection<EventItem> EventItems { get; set; }


        [Display(Name = "Member Attendance ID")]
        [Required(ErrorMessage = "Member Attendence ID Required")]
        [StringLength(36)]
        public string MemberAttendanceID { get; set; }
        public virtual AttendanceTracking AttendanceTracking { get; set; }    //Is this going to be a problem
        //public ICollection<AttendanceTracking> AttendanceTrackings { get; set; }

        #endregion
    }
}

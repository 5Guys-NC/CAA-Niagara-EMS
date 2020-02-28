using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
/******************************
*  Repository Created By: Jon Yade
*  Edited by:
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Interface for the AttendanceTracking Repository
    /// </summary>
    public interface IAttendanceTrackingRepository
    {
        List<AttendanceTracking> GetAttendanceTrackings();
        AttendanceTracking GetAttendanceTracking(string attendanceTrackingID);
        List<AttendanceTracking> GetAttendanceTrackingByEvent(string eventID);   //may need to add this for other respositories
        void AddAttendanceTracking(AttendanceTracking attendanceTrackingToAdd);
        void UpdateAttendanceTracking(AttendanceTracking attendanceTrackingToUpdate);
        void DeleteAttendanceTracking(AttendanceTracking attendanceTrackingToDelete);
    }
}
using CAA_Event_Management.Models;
using System.Collections.Generic;
using System.Linq;
/******************************
*  Created By: Jon Yade
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// AttendanceTracking Repository that contains the CRUD functions for the AttendanceTracking Table
    /// </summary>
    public class AttendanceTrackingRepository : IAttendanceTrackingRepository
    {
        #region Get Requests

        /// <summary>
        /// Get all AttendanceTrackings
        /// </summary>
        /// <returns>List of ATTENDANCETRACKINGS</returns>
        public List<AttendanceTracking> GetAttendanceTrackings()
        {
            using (CAAContext context = new CAAContext())
            {
                var attendanceTrackings = context.AttendanceTrackings
                    .OrderBy(d => d.EventID)  //should probably take this out
                    .ThenBy(d => d.MemberAttendanceID)
                    .ToList();
                return attendanceTrackings;
            }
        }

        /// <summary>
        /// Get AttendanceTracking by ID
        /// </summary>
        /// <param name="memberAttendanceID"></param>
        /// <returns>a Single ATTENDANCETRACKING</returns>
        public AttendanceTracking GetAttendanceTracking(string memberAttendanceID)
        {
            using (CAAContext context = new CAAContext())
            {
                var attendanceTracking = context.AttendanceTrackings
                    .Where(d => d.MemberAttendanceID == memberAttendanceID)
                    .FirstOrDefault();
                return attendanceTracking;
            }
        }

        /// <summary>
        /// Get AttendanceTrackings by EventID
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns>List of ATTENDANCETRACKINGS</returns>
        public List<AttendanceTracking> GetAttendanceTrackingByEvent(string eventID)
        {
            using (CAAContext context = new CAAContext())
            {
                var attendanceTracking = context.AttendanceTrackings
                    .Where(d => d.EventID == eventID)
                    .ToList();
                return attendanceTracking;
            }
        }

        /// <summary>
        /// Get Last Attendance by Event
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
        public AttendanceTracking GetLastAttendanceTrackingByEvent(string eventID)
        {
            using (CAAContext context = new CAAContext())
            {
                var lastAttendee = context.AttendanceTrackings
                    .Where(p => p.EventID == eventID)
                    .LastOrDefault();
                return lastAttendee;
            }
        }

        #endregion

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="attendanceTrackingToAdd"></param>
        public void AddAttendanceTracking(AttendanceTracking attendanceTrackingToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.AttendanceTrackings.Add(attendanceTrackingToAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="attendanceTrackingToUpdate"></param>
        public void UpdateAttendanceTracking(AttendanceTracking attendanceTrackingToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(attendanceTrackingToUpdate);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="attendanceTrackingToDelete"></param>
        public void DeleteAttendanceTracking(AttendanceTracking attendanceTrackingToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.AttendanceTrackings.Remove(attendanceTrackingToDelete);
                context.SaveChanges();
            }
        }
    }
}

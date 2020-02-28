using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
/******************************
*  Repository Created By: Jon Yade
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// AttendanceTracking Repository that contains the CRUD functions for the AttendanceTracking Table
    /// </summary>
    public class AttendanceTrackingRepository : IAttendanceTrackingRepository
    {
        #region Get
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
        #endregion

        #region Add
        public void AddAttendanceTracking(AttendanceTracking attendanceTrackingToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.AttendanceTrackings.Add(attendanceTrackingToAdd);
                context.SaveChanges();
            }
        }
        #endregion

        #region Update
        public void UpdateAttendanceTracking(AttendanceTracking attendanceTrackingToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(attendanceTrackingToUpdate);
                context.SaveChanges();
            }
        }
        #endregion

        #region Delete
        public void DeleteAttendanceTracking(AttendanceTracking attendanceTrackingToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.AttendanceTrackings.Remove(attendanceTrackingToDelete);
                context.SaveChanges();
            }
        }
        #endregion
    }
}

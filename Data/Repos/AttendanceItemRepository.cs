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
    /// AttendanceItem Repository that contains the CRUD functions for the AttendanceItem Table
    /// </summary>
    public class AttendanceItemRepository : IAttendanceItemRepository
    {
        #region Get Requests

        /// <summary>
        /// Get all AttendanceItems
        /// </summary>
        /// <returns>List of ATTENDANCEITEMS</returns>
        public List<AttendanceItem> GetAttendanceItems()
        {
            using (CAAContext context = new CAAContext())
            {
                var AttendanceItems = context.AttendanceItems
                    .OrderBy(d => d.MemberAttendanceID)
                    .ToList();
                return AttendanceItems;
            }
        }

        /// <summary>
        /// Get AttendanceItem by ID
        /// </summary>
        /// <param name="attendanceItemID"></param>
        /// <returns>A Single ATTENDANCEITEMS</returns>
        public AttendanceItem GetAttendanceItem(string attendanceItemID)
        {
            using (CAAContext context = new CAAContext())
            {
                var attendanceItem = context.AttendanceItems
                    .Where(d => d.MemberAttendanceID == attendanceItemID)
                    .FirstOrDefault();
                return attendanceItem;
            }
        }
        #endregion

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="attendanceItemToAdd"></param>
        public void AddAttendanceItem(AttendanceItem attendanceItemToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.AttendanceItems.Add(attendanceItemToAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="attendanceItemToUpdate"></param>
        public void UpdateAttendanceItem(AttendanceItem attendanceItemToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(attendanceItemToUpdate);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="attendanceItemToDelete"></param>
        public void DeleteAttendanceItem(AttendanceItem attendanceItemToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.AttendanceItems.Remove(attendanceItemToDelete);
                context.SaveChanges();
            }
        }
    }
}

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
    /// AttendanceItem Repository that contains the CRUD functions for the AttendanceItem Table
    /// </summary>
    public class AttendanceItemRepository : IAttendanceItemRepository
    {
        #region Get
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

        #region Add
        public void AddAttendanceItem(AttendanceItem attendanceItemToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.AttendanceItems.Add(attendanceItemToAdd);
                context.SaveChanges();
            }
        }
        #endregion

        #region Update
        public void UpdateAttendanceItem(AttendanceItem attendanceItemToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(attendanceItemToUpdate);
                context.SaveChanges();
            }
        }
        #endregion

        #region Delete
        public void DeleteAttendanceItem(AttendanceItem attendanceItemToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.AttendanceItems.Remove(attendanceItemToDelete);
                context.SaveChanges();
            }
        }
        #endregion
    }
}

using CAA_Event_Management.Models;
using System.Collections.Generic;
/******************************
*  Created By: Jon Yade
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Interface for the AttendanceItem Repository
    /// </summary>
    public interface IAttendanceItemRepository
    {
        List<AttendanceItem> GetAttendanceItems();
        AttendanceItem GetAttendanceItem(string attendanceItemID);
        void AddAttendanceItem(AttendanceItem attendanceItemToAdd);
        void UpdateAttendanceItem(AttendanceItem attendanceItemToUpdate);
        void DeleteAttendanceItem(AttendanceItem attendanceItemToDelete);
    }
}
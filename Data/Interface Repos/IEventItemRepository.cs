using CAA_Event_Management.Models;
using System.Collections.Generic;
/******************************
*  Created By: Jon Yade
*  Edited by:
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Interface for the EventItem Repository
    /// </summary>
    public interface IEventItemRepository
    {
        List<EventItem> GetEventItems();
        List<EventItem> GetEventItems(string eventID);
        EventItem GetEventItem(string eventItemID);
        EventItem GetEventItem(string eventID, string itemID);
        void AddEventItem(EventItem eventItemToAdd);
        void UpdateEventItem(EventItem eventItemToUpdate);
        void DeleteEventItem(EventItem eventItemToDelete);
    }
}
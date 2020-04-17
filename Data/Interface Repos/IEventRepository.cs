using CAA_Event_Management.Models;
using System.Collections.Generic;
/******************************
*  Created By: Jon Yade
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Interface for the Event Repository
    /// </summary>
    public interface IEventRepository
    {
        List<Event> GetEvents();
        List<Event> GetEvents(bool deleted);
        Event GetEvent(string eventID);
        Event GetEventByName(string eventName);
        Event GetLastEvent();
        void AddEvent(Event eventToAdd);
        void UpdateEvent(Event eventToUpdate);
        void DeleteEvent(Event eventToDelete);
        void DeleteEventPermanent(Event eventToDelete);
    }
}

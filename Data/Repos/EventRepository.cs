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
    /// Event Repository that contains the CRUD functions for the Event Table
    /// </summary>
    public class EventRepository : IEventRepository
    {
        #region Get Requests

        /// <summary>
        /// Get all Events
        /// </summary>
        /// <returns>List of EVENTS</returns>
        public List<Event> GetEvents()
        {
            using (CAAContext context = new CAAContext())
            {
                var events = context.Events
                    //.OrderByDescending(d => d.EventStart)
                    //.ThenBy(d => d.EventName)
                    .ToList();
                return events;
            }
        }

        /// <summary>
        /// Get Events by Delete Status
        /// </summary>
        /// <param name="deleted"></param>
        /// <returns>List of EVENTS</returns>
        public List<Event> GetEvents(bool deleted)
        {
            using (CAAContext context = new CAAContext())
            {
                var events = context.Events
                    .Where(d => d.IsDeleted == deleted)
                    .OrderBy(d => d.EventName)
                    .ToList();
                return events;
            }
        }

        /// <summary>
        /// Get Event by ID
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns>A Single EVENT</returns>
        public Event GetEvent(string eventID)
        {
            using (CAAContext context = new CAAContext())
            {
                var oneEvent = context.Events
                    .Where(d => d.EventID == eventID)
                    .FirstOrDefault();
                return oneEvent;
            }
        }

        /// <summary>
        /// Get Event by Name
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns>A Single EVENT</returns>
        public Event GetEventByName(string eventName)
        {
            using (CAAContext context = new CAAContext())
            {
                var oneEvent = context.Events
                    .Where(d => d.EventName == eventName)
                    .FirstOrDefault();
                return oneEvent;
            }
        }

        /// <summary>
        /// Get Last Event
        /// </summary>
        /// <returns>A Single EVENT</returns>
        public Event GetLastEvent()
        {
            using (CAAContext context = new CAAContext())
            {
                var lastEvent = context.Events
                    .LastOrDefault();
                return lastEvent;
            }
        }
        #endregion

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="eventToAdd"></param>
        public void AddEvent(Event eventToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Events.Add(eventToAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="eventToUpdate"></param>
        public void UpdateEvent(Event eventToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(eventToUpdate);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete (but kept in Database)
        /// </summary>
        /// <param name="eventToDelete"></param>
        public void DeleteEvent(Event eventToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                Event thisEvent = GetEvent(eventToDelete.EventID);
                thisEvent.IsDeleted = true;
                context.Update(thisEvent);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Permanent Delete (No Longer in Database)
        /// </summary>
        /// <param name="eventToDelete"></param>
        public void DeleteEventPermanent(Event eventToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Events.Remove(eventToDelete);
                context.SaveChanges();
            }
        }
    }
}

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
    /// Event Repository that contains the CRUD functions for the Event Table
    /// </summary>
    public class EventRepository : IEventRepository
    {
        #region Get
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

        #region Add
        public void AddEvent(Event eventToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Events.Add(eventToAdd);
                context.SaveChanges();
            }
        }
        #endregion

        #region Update
        public void UpdateEvent(Event eventToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(eventToUpdate);
                context.SaveChanges();
            }
        }
        #endregion

        #region Delete
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

        public void DeleteEventPermanent(Event eventToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Events.Remove(eventToDelete);
                context.SaveChanges();
            }
        }
        #endregion
    }
}

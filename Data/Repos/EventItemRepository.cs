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
    /// EventItems Repository that contains the CRUD functions for the EventItems Table
    /// </summary>
    public class EventItemRepository : IEventItemRepository
    {
        #region Get Requests

        /// <summary>
        /// Get all EventItems
        /// </summary>
        /// <returns>List of EVENTITEMS</returns>
        public List<EventItem> GetEventItems()
        {
            using (CAAContext context = new CAAContext())
            {
                var eventItems = context.EventItems
                    .OrderBy(d => d.EventItemID)
                    .ToList();
                return eventItems;
            }
        }

        /// <summary>
        /// Get EventItems by EventID
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns>List of EVENTITEMS</returns>
        public List<EventItem> GetEventItems(string eventID)
        {
            using (CAAContext context = new CAAContext())
            {
                var eventItems = context.EventItems
                    .Where(d => d.EventID == eventID)
                    .OrderBy(d => d.EventItemID)
                    .ToList();
                return eventItems;
            }
        }

        /// <summary>
        /// Get EventItem by ID
        /// </summary>
        /// <param name="eventItemID"></param>
        /// <returns>A Single EVENTITEM</returns>
        public EventItem GetEventItem(string eventItemID)
        {
            using (CAAContext context = new CAAContext())
            {
                var eventItem = context.EventItems
                    .Where(d => d.EventItemID == eventItemID)
                    .FirstOrDefault();
                return eventItem;
            }
        }

        /// <summary>
        /// Get EventItem by EventID, ItemID combo
        /// </summary>
        /// <param name="eventID"></param>
        /// <param name="itemID"></param>
        /// <returns>A single EVENTITEM</returns>
        public EventItem GetEventItem(string eventID, string itemID)
        {
            using (CAAContext context = new CAAContext())
            {
                var eventItem = context.EventItems
                    .Where(d => d.EventID == eventID)
                    .Where(d => d.ItemID == itemID)
                    .FirstOrDefault();
                return eventItem;
            }
        }
        #endregion

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="eventItemToAdd"></param>
        public void AddEventItem(EventItem eventItemToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.EventItems.Add(eventItemToAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="eventItemToUpdate"></param>
        public void UpdateEventItem(EventItem eventItemToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(eventItemToUpdate);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="eventItemToDelete"></param>
        public void DeleteEventItem(EventItem eventItemToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.EventItems.Remove(eventItemToDelete);
                context.SaveChanges();
            }
        }
    }
}

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
    /// EventItems Repository that contains the CRUD functions for the EventItems Table
    /// </summary>
    public class EventItemRepository : IEventItemRepository
    {
        #region Get
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

        #region Add

        public void AddEventItem(EventItem eventItemToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.EventItems.Add(eventItemToAdd);
                context.SaveChanges();
            }
        }
        #endregion

        #region Update
        public void UpdateEventItem(EventItem eventItemToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(eventItemToUpdate);
                context.SaveChanges();
            }
        }
        #endregion

        #region Delete
        public void DeleteEventItem(EventItem eventItemToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.EventItems.Remove(eventItemToDelete);
                context.SaveChanges();
            }
        }
        #endregion
    }
}

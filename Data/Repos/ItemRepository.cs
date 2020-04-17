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
    /// Item Repository that contains the CRUD functions for the Item Table
    /// </summary>
    public class ItemRepository : IItemRepository
    {
        #region Get Requests

        /// <summary>
        /// Get all Items
        /// </summary>
        /// <returns>List of ITEMS</returns>
        public List<Item> GetItems()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Items
                    .OrderBy(d => d.ItemName)
                    .ToList();
                return items;
            }
        }

        /// <summary>
        /// Get undelete Items
        /// </summary>
        /// <returns></returns>
        public List<Item> GetUndeletedItems()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Items
                    .Where(d => d.IsDeleted == false || d.IsDeleted == null)
                    .OrderBy(d => d.ItemName)
                    .ToList();
                return items;
            }
        }

        /// <summary>
        /// Get Undeleted Items by Count
        /// </summary>
        /// <returns></returns>
        public List<Item> GetUndeletedItemsByCount()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Items
                    .Where(d => d.IsDeleted == false || d.IsDeleted == null)
                    .OrderByDescending(d => d.ItemCount)
                    .ThenBy(d => d.ItemName)
                    .ToList();
                return items;
            }
        }
        /// <summary>
        /// Get Item by ID
        /// </summary>
        /// <param name="itemID"></param>
        /// <returns>A Single ITEM</returns>
        public Item GetItem(string itemID)
        {
            using (CAAContext context = new CAAContext())
            {
                var item = context.Items
                    .Where(d => d.ItemID == itemID)
                    .FirstOrDefault();
                return item;
            }
        }
        #endregion
       
        #region CRUD operations
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="itemToAdd"></param>
        public void AddItem(Item itemToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Items.Add(itemToAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="itemToUpdate"></param>
        public void UpdateItem(Item itemToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(itemToUpdate);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update Item Count
        /// </summary>
        /// <param name="itemID"></param>
        /// <param name="countChange"></param>
        public void UpdateItemCount(string itemID, int countChange)
        {
            using (CAAContext context = new CAAContext())
            {
                var item = context.Items
                    .Where(d => d.ItemID == itemID)
                    .FirstOrDefault();
                item.ItemCount += countChange;
                context.Update(item);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete Update Item
        /// </summary>
        /// <param name="itemToDelete"></param>
        public void DeleteUpdateItem(Item itemToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(itemToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="itemToDelete"></param>
        public void DeleteItem(Item itemToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Items.Remove(itemToDelete);
                context.SaveChanges();
            }
        }
        #endregion
    }
}

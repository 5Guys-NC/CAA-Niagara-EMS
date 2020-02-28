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
    /// Item Repository that contains the CRUD functions for the Item Table
    /// </summary>
    public class ItemRepository : IItemRepository
    {
        #region Get
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

        #region Add
        public void AddItem(Item itemToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Items.Add(itemToAdd);
                context.SaveChanges();
            }
        }
        #endregion

        #region Update
        public void UpdateItem(Item itemToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(itemToUpdate);
                context.SaveChanges();
            }
        }
        #endregion

        #region Delete
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

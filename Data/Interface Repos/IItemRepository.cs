using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
/******************************
*  Repository Created By: Jon Yade
*  Edited by:
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Interface for the Item Repository
    /// </summary>
    public interface IItemRepository
    {
        List<Item> GetItems();
        Item GetItem(string itemID);
        void AddItem(Item itemToAdd);
        void UpdateItem(Item itemToUpdate);
        void DeleteItem(Item itemToDelete);
    }
}
using CAA_Event_Management.Models;
using System.Collections.Generic;
/******************************
*  Created By: Brian Culp
*  Edited by:
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Interface for the Users Repository
    /// </summary>
    public interface IUsersRepository
    {
        List<User> GetUsers();

        User GetUser(int ID);

        User GetUser(string userName);

        void AddUser(User userToAdd);

        void UpdateUser(User userToUpdate);

        void DeleteUser(User userToDelete);
    }
}

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
        List<Users> GetUsers();

        Users GetUser(int ID);

        Users GetUser(string userName);

        void AddUser(Users userToAdd);

        void UpdateUser(Users userToUpdate);

        void DeleteUser(Users userToDelete);
    }
}

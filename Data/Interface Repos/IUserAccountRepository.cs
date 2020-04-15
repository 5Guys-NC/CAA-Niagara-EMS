using CAA_Event_Management.Models;
using System.Collections.Generic;
/******************************
 * Created By: Brian Culp 95%
 * Edited By: Jon Yade 5%
 * ***************************/

namespace CAA_Event_Management.Data.Interface_Repos
{
    /// <summary>
    /// Interface for the UserAccount Repository
    /// </summary>
    interface IUserAccountRepository
    {
        List<UserAccount> GetUsers();

        UserAccount GetUserByID(string ID);

        UserAccount GetUser(string userName);

        void AddUser(UserAccount userToAdd);

        void UpdateUser(UserAccount userToUpdate);

        void DeleteUser(UserAccount userToDelete);
    }
}

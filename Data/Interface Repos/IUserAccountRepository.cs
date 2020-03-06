using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;

namespace CAA_Event_Management.Data.Interface_Repos
{
    interface IUserAccountRepository
    {
        List<UserAccount> GetUsers();

        UserAccount GetUser(int ID);

        UserAccount GetUser(string userName);

        void AddUser(UserAccount userToAdd);

        void UpdateUser(UserAccount userToUpdate);

        void DeleteUser(UserAccount userToDelete);
    }
}

using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Models;
using System.Collections.Generic;
using System.Linq;
/*********************************
 * Created By: Brian Culp
 * Edited By: Jon Yade
 * *******************************/

namespace CAA_Event_Management.Data.Repos
{
    /// <summary>
    /// Repository for CRUD of UserAccount
    /// </summary>
    public class UserAccountRepository : IUserAccountRepository
    {
        /// <summary>
        /// Add
        /// </summary>
        /// <param name="userToAdd"></param>
        public void AddUser(UserAccount userToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.UserAccounts.Add(userToAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="userToDelete"></param>
        public void DeleteUser(UserAccount userToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.UserAccounts.Remove(userToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get User by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A Single USER</returns>
        public UserAccount GetUserByID(string ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var u = context.UserAccounts
                    .Where(d => d.ID == ID)
                    .FirstOrDefault();
                return u;
            }
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns>List of USERS</returns>
        public List<UserAccount> GetUsers()
        {
            using (CAAContext context = new CAAContext())
            {
                var users = context.UserAccounts
                    .OrderBy(d => d.UserName)
                    .ToList();
                return users;
            }
        }

        /// <summary>
        /// Get User by UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>A Single USER</returns>
        public UserAccount GetUser(string userName)
        {
            using (CAAContext context = new CAAContext())
            {
                var users = context.UserAccounts
                    .Where(d => d.UserName == userName)
                    .FirstOrDefault();

                return users;
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userToUpdate"></param>
        public void UpdateUser(UserAccount userToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(userToUpdate);
                context.SaveChanges();
            }
        }
    }
}

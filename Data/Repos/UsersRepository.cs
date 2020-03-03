using CAA_Event_Management.Models;
using System.Collections.Generic;
using System.Linq;
/******************************
*  Created By: Brian Culp
*  Edited by:
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Users Repository that contains the CRUD functions for the User Table
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        /// <summary>
        /// ADD
        /// </summary>
        /// <param name="userToAdd"></param>
        public void AddUser(User userToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Users.Add(userToAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="userToDelete"></param>
        public void DeleteUser(User userToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Users.Remove(userToDelete);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get User by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A Single USER</returns>
        public User GetUser(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var u = context.Users
                    .Where(d => d.ID == ID)
                    .FirstOrDefault();
                return u;
            }
        }

        /// <summary>
        /// Get all Users
        /// </summary>
        /// <returns>List of USERS</returns>
        public List<User> GetUsers()
        {
           using (CAAContext context = new CAAContext())
            {
                var users = context.Users
                    .OrderBy(d=>d.UserName)
                    .ToList();
                return users;
            }
        }

        /// <summary>
        /// Get User by UserName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>A Single USER</returns>
        public User GetUser(string userName)
        {
            using (CAAContext context = new CAAContext())
            {
                var users = context.Users
                    .Where(d => d.UserName == userName)
                    .FirstOrDefault();
                    
                return users;
            }
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="userToUpdate"></param>
        public void UpdateUser(User userToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(userToUpdate);
                context.SaveChanges();
            }
        }
    }
}

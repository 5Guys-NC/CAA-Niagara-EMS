using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
/******************************
*  Repository Created By: Brian Culp
*  Edited by:
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Users Repository that contains the CRUD functions for the User Table
    /// </summary>
    public class UsersRepository : IUsersRepository
    {
        public void AddUser(Users userToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Users.Add(userToAdd);
                context.SaveChanges();
            }
        }

        public void DeleteUser(Users userToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Users.Remove(userToDelete);
                context.SaveChanges();
            }
        }

        public Users GetUser(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var u = context.Users
                    .Where(d => d.ID == ID)
                    .FirstOrDefault();
                return u;
            }
        }

        public List<Users> GetUsers()
        {
           using (CAAContext context = new CAAContext())
            {
                var users = context.Users
                    .OrderBy(d=>d.UserName)
                    .ToList();
                return users;
            }
        }

        public Users GetUser(string userName)
        {
            using (CAAContext context = new CAAContext())
            {
                var users = context.Users
                    .Where(d => d.UserName == userName)
                    .FirstOrDefault();
                    
                return users;
            }
        }

        public void UpdateUser(Users userToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(userToUpdate);
                context.SaveChanges();
            }
        }
    }
}

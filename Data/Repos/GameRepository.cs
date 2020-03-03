using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
using Microsoft.EntityFrameworkCore;
/******************************
*  Repository Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Question Repository that contains the CRUD functions for the Game Table
    /// </summary>
    public class GameRepository : IGameRepository
    {
        #region Get
        public List<Game> GetGame(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var game = context.Games
                    .Where(g => g.ID == ID).Include(q => q.Questions).ToList();
                return game;
            }
        }

        public Game GetAGame(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var game = context.Games
                    .Where(g => g.ID == ID).Include(q => q.Questions).FirstOrDefault();
                return game;
            }
        }

        public List<Game> GetGames()
        {
            using (CAAContext context = new CAAContext())
            {
                var games = context.Games.OrderBy(g => g.ID).ToList();
                return games;
            }
        }
        #endregion

        #region Add
        public void AddGame(Game gameToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Games.Add(gameToAdd);
                context.SaveChanges();
            }
        }
        #endregion

        #region Delete
        public void DeleteGame(Game gameToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Games.Remove(gameToDelete);
                context.SaveChanges();
            }
        }
        #endregion

        #region Update
        public void UpdateGame(Game gameToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Games.Update(gameToUpdate);
                context.SaveChanges();
            }
        }
        #endregion

        #region Search
        public List<Game> SearchGame(string search)
        {
            using (CAAContext context = new CAAContext())
            {
                var question = context.Games
                    .Where(d => d.Title.ToUpper().Contains(search.ToUpper())).ToList();
                return question;
            }
        }
        #endregion
    }
}

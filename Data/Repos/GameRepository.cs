using CAA_Event_Management.Models;
using System.Collections.Generic;
using System.Linq;
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
        /// <summary>
        /// Get all Games
        /// </summary>
        /// <returns></returns>
        public List<Game> GetGames()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Games.OrderBy(o => o.CreatedDate).ToList();
                return items;
            }
        }

        /// <summary>
        /// Save Game Model
        /// </summary>
        /// <param name="gmToSave"></param>
        public void SaveGameModel(GameModel gmToSave)
        {
            using (CAAContext context = new CAAContext())
            {
                context.GameModels.Add(gmToSave);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Add Game
        /// </summary>
        /// <param name="toAdd"></param>
        public void AddGame(Game toAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Games.Add(toAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Get Game by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Game GetGame(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Games.Where(g => g.ID == ID).FirstOrDefault();
                return items;
            }
        }

        /// <summary>
        /// Remove Game
        /// </summary>
        /// <param name="game"></param>
        public void RemoveGame(Game game)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Remove(game);
                context.SaveChanges();
            }
        }
    }
}

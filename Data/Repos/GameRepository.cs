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
        public List<Game> GetGames()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Games.OrderBy(o => o.CreatedDate).ToList();
                return items;
            }
        }

        public void SaveGameModel(GameModel gmToSave)
        {
            using (CAAContext context = new CAAContext())
            {
                context.GameModels.Add(gmToSave);
                context.SaveChanges();
            }
        }

        public void AddGame(Game toAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Games.Add(toAdd);
                context.SaveChanges();
            }
        }
        public Game GetGame(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Games.Where(g => g.ID == ID).FirstOrDefault();
                return items;
            }
        }
    }
}

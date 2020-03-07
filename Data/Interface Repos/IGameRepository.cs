using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
/******************************
*  Repository Created By: Max Cashmore
*  Edited by:
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Interface for the Game Repository
    /// </summary>
    public interface IGameRepository
    {
        List<Game> GetGames();
        void SaveGameModel(GameModel gmToSave);
        void AddGame(Game toAdd);
        Game GetGame(int ID);
        void RemoveGame(Game game);
    }
}

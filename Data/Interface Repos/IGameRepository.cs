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
        List<Game> GetGame(int ID);
        Game GetAGame(int ID);
        void AddGame(Game gameToAdd);
        void UpdateGame(Game gameToUpdate);
        void DeleteGame(Game gameToDelete);
        List<Game> SearchGame(String search);
    }
}

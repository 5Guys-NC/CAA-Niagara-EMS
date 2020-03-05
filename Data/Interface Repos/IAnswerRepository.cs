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
    /// Interface for the Answer Repository
    /// </summary>
    public interface IAnswerRepository
    {
        List<Answer> GetAnswers();
        GameModel GetGameModel(int ID);
        List<Answer> GetAnswerSelection();
        Answer GetAnswer(int id);
        void RemoveAnswer(Answer answer);
        void AddAnswer(Answer toAdd);
        void UpdateGM(GameModel UpdateGM);
    }
}

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
    /// Interface for the Question Repository
    /// </summary>
    public interface IQuestionRepository
    {
        List<Question> GetQuestions();
        List<GameModel> GetModelQuestions(int ID);
        GameModel GetModelQuestion(int ID);
        List<Question> GetQuestionSelection();
        void AddQuestion(Question toAdd);
        void RemoveGameModel(GameModel toRemove);
    }
}

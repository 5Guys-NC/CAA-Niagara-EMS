using CAA_Event_Management.Models;
using System.Collections.Generic;
/******************************
*  Created By: Max Cashmore
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
        Question GetQuestion(int id);
        List<GameModel> GetModelQuestions(int ID);
        GameModel GetModelQuestion(int ID);
        List<Question> GetQuestionSelection();
        void AddQuestion(Question toAdd);
        void RemoveGameModel(GameModel toRemove);
        void RemoveQuestion(Question toRemove);
    }
}

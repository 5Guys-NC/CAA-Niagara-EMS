using CAA_Event_Management.Models;
using System.Collections.Generic;
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

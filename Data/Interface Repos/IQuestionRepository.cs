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
        List<Question> GetQuestionsByGame(int ID);
        Question GetQuestion(int ID);
        void AddQuestion(Question queToAdd);
        void UpdateQuestion(Question queToUpdate);
        void DeleteQuestion(Question queToDelete);
        List<Question> SearchQuestion(string search);
    }
}

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
        List<Answer> GetAnswersByQuestion(int ID);
        Answer GetAnswer(int ID);
        void AddAnswers(List<Answer> ansToAdd);
        void AddAnswer(Answer anToAdd);
        void UpdateAnswer(Answer anToupdate);
        void UpdateAnswers(List<Answer> ansToupdate);
        void DeleteAnswer(Answer ansToDelete);
        List<Answer> SearchAnswer(string search);
    }
}

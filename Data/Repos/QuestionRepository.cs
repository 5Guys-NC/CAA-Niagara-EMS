using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
/******************************
*  Repository Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Question Repository that contains the CRUD functions for the Question Table
    /// </summary>
    public class QuestionRepository : IQuestionRepository
    {
        #region Get Requests

        /// <summary>
        /// Get All Question
        /// </summary>
        /// <returns></returns>
        public List<Question> GetQuestions()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Questions
                    .OrderBy(d => d.ID)
                    .ToList();
                return items;
            }
        }

        public List<Question> GetQuestionsByGame(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Questions
                    .Where(d => d.GameID == ID).ToList();
                return items;
            }
        }

        public Question GetQuestion(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var question = context.Questions
                    .Where(d => d.ID == ID).FirstOrDefault();
                return question;
            }
        }
        #endregion

        #region Add
        public void AddQuestion(Question queToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Questions.Add(queToAdd);
                context.SaveChanges();
            }
        }
        #endregion

        #region Update
        public void UpdateQuestion(Question queToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(queToUpdate);
                context.SaveChanges();
            }
        }
        #endregion

        #region Delete
        public void DeleteQuestion(Question queToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Questions.Remove(queToDelete);
                context.SaveChanges();
            }
        }
        #endregion

        #region Search
        public List<Question> SearchQuestion(string search)
        {
            using (CAAContext context = new CAAContext())
            {
                var question = context.Questions
                    .Where(d => d.Phrase.ToUpper().Contains(search.ToUpper())).ToList();
                return question;
            }
        }
        #endregion
    }
}

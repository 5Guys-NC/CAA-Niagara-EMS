using CAA_Event_Management.Models;
using System.Collections.Generic;
using System.Linq;
/******************************
*  Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Question Repository that contains the CRUD functions for the Question Table
    /// </summary>
    public class QuestionRepository : IQuestionRepository
    {
        /// <summary>
        /// Get all Questions
        /// </summary>
        /// <returns></returns>
        public List<Question> GetQuestions()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Questions.OrderByDescending(o => o.TimesUsed).ToList();
                return items;
            }
        }

        /// <summary>
        /// Get Question by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Question GetQuestion(int id)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Questions.Where(q=>q.ID == id).FirstOrDefault();
                return items;
            }
        }

        /// <summary>
        /// Get Model Questions by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>multiple</returns>
        public List<GameModel> GetModelQuestions(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.GameModels.Where(i => i.GameID == ID).ToList();
                return items;
            }
        }

        /// <summary>
        /// Get Model Questions by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>single</returns>
        public GameModel GetModelQuestion(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.GameModels.Where(i => i.ID == ID).FirstOrDefault();
                return items;
            }
        }

        /// <summary>
        /// Get Question Selection
        /// </summary>
        /// <returns></returns>
        public List<Question> GetQuestionSelection()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Questions.OrderBy(o => o.TimesUsed).ToList();
                return items;
            }
        }

        /// <summary>
        /// Add Question
        /// </summary>
        /// <param name="toAdd"></param>
        public void AddQuestion(Question toAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Questions.Add(toAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Remove Game Model
        /// </summary>
        /// <param name="toRemove"></param>
        public void RemoveGameModel(GameModel toRemove)
        {
            using (CAAContext context = new CAAContext())
            {
                context.GameModels.Remove(toRemove);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Remove Question
        /// </summary>
        /// <param name="toRemove"></param>
        public void RemoveQuestion(Question toRemove)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Questions.Remove(toRemove);
                context.SaveChanges();
            }
        }
    }
}

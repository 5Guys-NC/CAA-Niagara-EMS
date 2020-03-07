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
    /// Answer Repository that contains the CRUD functions for the Answer Table
    /// </summary>
    public class AnswerRepository : IAnswerRepository
    {
        #region Get Requests

        /// <summary>
        /// Get All Answers
        /// </summary>
        /// <returns>List of ANSWERS</returns>
        public List<Answer> GetAnswers()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Answers.OrderByDescending(o => o.TimesUsed).ToList();
                return items;
            }
        }

        public Answer GetAnswer(int id)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Answers.Where(a => a.ID == id).FirstOrDefault();
                return items;
            }
        }

        /// <summary>
        /// Get Game Model by ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>A single GameModel</returns>
        public GameModel GetGameModel(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.GameModels.Where(i => i.ID == ID).FirstOrDefault();
                return items;
            }
        }

        /// <summary>
        /// Displays Answers not already in the question
        /// Work in progress
        /// </summary>
        /// <returns>List of ANSWERS</returns>
        public List<Answer> GetAnswerSelection()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Answers.OrderBy(o => o.TimesUsed).ToList();
                return items;
            }
        }

        #endregion

        /// <summary>
        /// Add Answer
        /// </summary>
        /// <param name="toAdd"></param>
        public void AddAnswer(Answer toAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Answers.Add(toAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Update GameModel
        /// </summary>
        /// <param name="UpdateGM"></param>
        public void UpdateGM(GameModel UpdateGM)
        {
            using (CAAContext context = new CAAContext())
            {
                context.GameModels.Update(UpdateGM);
                context.SaveChanges();
            }
        }

        public void RemoveAnswer(Answer answer)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Answers.Remove(answer);
                context.SaveChanges();
            }
        }
    }
}
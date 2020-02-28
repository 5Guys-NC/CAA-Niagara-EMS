using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
using Microsoft.EntityFrameworkCore;
/******************************
*  Repository Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Answer Repository that contains the CRUD functions for the Answer Table
    /// </summary>
    public class AnswerRepository : IAnswerRepository
    {
        #region Get
        public List<Answer> GetAnswers()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Answers
                    .OrderBy(d => d.ID)
                    .ToList();
                return items;
            }
        }
        public List<Answer> GetAnswersByQuestion(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var answer = context.Answers
                    .Where(d => d.QuestionID == ID).ToList();
                return answer;
            }
        }
        public Answer GetAnswer(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var answer = context.Answers
                    .Where(d => d.ID == ID).FirstOrDefault();
                return answer;
            }
        }
        #endregion

        #region Add

        //When adding multiple answers
        public void AddAnswers(List<Answer> ansToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                foreach (Answer a in ansToAdd)
                {
                    context.Answers.Add(a);

                }
                context.SaveChanges();
            }
        }

        public void AddAnswer(Answer anToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Answers.Add(anToAdd);
                context.SaveChanges();
            }
        }
        #endregion
        
        #region Update

        public void UpdateAnswer(Answer anToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Answers.Update(anToUpdate);
                context.SaveChanges();
            }
        }
        public void UpdateAnswers(List<Answer> ansToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                foreach (Answer a in ansToUpdate)
                {
                    context.Answers.Update(a);
                    context.SaveChanges();
                }
            }
        }
        #endregion
        
        #region Delete

        public void DeleteAnswer(Answer ansToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Answers.Remove(ansToDelete);
                context.SaveChanges();
            }
        }
        #endregion
        
        #region Search
        public List<Answer> SearchAnswer(string search)
        {

            using (CAAContext context = new CAAContext())
            {
                var answer = context.Answers
                    .Where(d => d.Phrase.ToUpper().Contains(search.ToUpper())).ToList();
                return answer;
            }

        }
        #endregion
    }
}
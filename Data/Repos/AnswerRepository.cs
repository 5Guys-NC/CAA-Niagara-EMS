using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;
using Microsoft.EntityFrameworkCore;
/******************************
*  Repository Created By: Max Cashmore

*******************************/
namespace CAA_Event_Management.Data
{
    /// <summary>
    /// Answer Repository that contains the CRUD functions for the Answer Table
    /// </summary>
    public class AnswerRepository : IAnswerRepository
    {
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
        /// <returns></returns>
        public List<Answer> GetAnswerSelection()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Answers.OrderBy(o => o.TimesUsed).ToList();
                return items;
            }
        }

        public void AddAnswer(Answer toAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Answers.Add(toAdd);
                context.SaveChanges();
            }
        }

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
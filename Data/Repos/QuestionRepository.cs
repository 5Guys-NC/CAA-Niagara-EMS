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
        public List<Question> GetQuestions()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Questions.OrderByDescending(o => o.TimesUsed).ToList();
                return items;
            }
        }

        public List<GameModel> GetModelQuestions(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.GameModels.Where(i => i.GameID == ID).ToList();
                return items;
            }
        }

        public GameModel GetModelQuestion(int ID)
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.GameModels.Where(i => i.ID == ID).FirstOrDefault();
                return items;
            }
        }

        public List<Question> GetQuestionSelection()
        {
            using (CAAContext context = new CAAContext())
            {
                var items = context.Questions.OrderBy(o => o.TimesUsed).ToList();
                return items;
            }
        }

        public void AddQuestion(Question toAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Questions.Add(toAdd);
                context.SaveChanges();
            }
        }

        public void RemoveGameModel(GameModel toRemove)
        {
            using (CAAContext context = new CAAContext())
            {
                context.GameModels.Remove(toRemove);
                context.SaveChanges();
            }
        }
    }
}

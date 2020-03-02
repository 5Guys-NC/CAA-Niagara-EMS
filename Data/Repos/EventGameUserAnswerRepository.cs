using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Models;

namespace CAA_Event_Management.Data.Repos
{
    class EventGameUserAnswerRepository:IEventGameUserAnswerRepository
    {
        public List<EventGameUserAnswer> GetEventGameUserAnswers()
        {
            using (CAAContext context = new CAAContext())
            {
                var answers = context.EventGameUserAnswers
                    //.OrderBy(d => d.)
                    .ToList();
                return answers;
            }
        }
        public List<EventGameUserAnswer> GetEventGameUserAnswers(string eventID)
        {
            using (CAAContext context = new CAAContext())
            {
                var answer = context.EventGameUserAnswers
                    .Where(d => d.EventID == eventID)
                    .ToList();
                return answer;
            }
        }

        public void AddEventGameUserAnswer(EventGameUserAnswer userAnswerToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.EventGameUserAnswers.Add(userAnswerToAdd);
                context.SaveChanges();
            }
        }
        public void UpdateEventGameUserAnswer(EventGameUserAnswer userAnswerToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(userAnswerToUpdate);
                context.SaveChanges();
            }
        }
        public void DeleleEventGameUserAnswer(EventGameUserAnswer userAnswerToDelete)
        {
            using (CAAContext context = new CAAContext())
            {
                context.EventGameUserAnswers.Remove(userAnswerToDelete);
                context.SaveChanges();
            }
        }
    }
}

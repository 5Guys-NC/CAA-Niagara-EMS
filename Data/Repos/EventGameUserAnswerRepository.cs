using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Models;
using System.Collections.Generic;
using System.Linq;
/*********************************
 * Created By: Max Cashmore
 ********************************/

namespace CAA_Event_Management.Data.Repos
{
    /// <summary>
    /// EventGameUserAnswer Repository that contains the CRUD functions for the EventGameUserAnswer Table
    /// </summary>
    class EventGameUserAnswerRepository :IEventGameUserAnswerRepository
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

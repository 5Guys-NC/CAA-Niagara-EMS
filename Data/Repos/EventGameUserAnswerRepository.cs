using CAA_Event_Management.Data.Interface_Repos;
using CAA_Event_Management.Models;
using System.Collections.Generic;
using System.Linq;
/*********************************
 * Created By: Max Cashmore
 * Edited By: Brian Culp
 ********************************/
namespace CAA_Event_Management.Data.Repos
{
    /// <summary>
    /// EventGameUserAnswer Repository that contains the CRUD functions for the EventGameUserAnswer Table
    /// </summary>
    class EventGameUserAnswerRepository :IEventGameUserAnswerRepository
    {
        /// <summary>
        /// Get All EventGameUserAnswers
        /// </summary>
        /// <returns></returns>
        public List<EventGameUserAnswer> GetEventGameUserAnswers()
        {
            using (CAAContext context = new CAAContext())
            {
                var answers = context.EventGameUserAnswers
                    .ToList();
                return answers;
            }
        }

        /// <summary>
        /// Get single EventGameUserAnswer by EventID
        /// </summary>
        /// <param name="eventID"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add EventGameUserAnswer
        /// </summary>
        /// <param name="userAnswerToAdd"></param>
        public void AddEventGameUserAnswer(EventGameUserAnswer userAnswerToAdd)
        {
            using (CAAContext context = new CAAContext())
            {
                context.EventGameUserAnswers.Add(userAnswerToAdd);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// UpdateEventGameUserAnswer
        /// </summary>
        /// <param name="userAnswerToUpdate"></param>
        public void UpdateEventGameUserAnswer(EventGameUserAnswer userAnswerToUpdate)
        {
            using (CAAContext context = new CAAContext())
            {
                context.Update(userAnswerToUpdate);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Delete EventGameUserAnswer
        /// </summary>
        /// <param name="userAnswerToDelete"></param>
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

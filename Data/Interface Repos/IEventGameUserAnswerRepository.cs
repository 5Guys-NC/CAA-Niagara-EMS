using CAA_Event_Management.Models;
using System.Collections.Generic;
/***************************
 * Created By: Max Cashmore
 * *************************/
namespace CAA_Event_Management.Data.Interface_Repos
{
    /// <summary>
    /// Interface for EventGameUserAnswer Repository
    /// </summary>
    public interface IEventGameUserAnswerRepository
    {
        List<EventGameUserAnswer> GetEventGameUserAnswers();
        List<EventGameUserAnswer> GetEventGameUserAnswers(string eventID);
        void AddEventGameUserAnswer(EventGameUserAnswer userAnswerToAdd);
        void UpdateEventGameUserAnswer(EventGameUserAnswer userAnswerToUpdate);
        void DeleleEventGameUserAnswer(EventGameUserAnswer userAnswerToDelete);
    }
}

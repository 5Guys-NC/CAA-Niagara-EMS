using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;

namespace CAA_Event_Management.Data.Interface_Repos
{
    public interface IEventGameUserAnswerRepository
    {
        List<EventGameUserAnswer> GetEventGameUserAnswers();
        List<EventGameUserAnswer> GetEventGameUserAnswers(string eventID);
        //EventGameUserAnswer GetEventGameUserAnswer(string eventGameUserAnsswerID);
        void AddEventGameUserAnswer(EventGameUserAnswer userAnswerToAdd);
        void UpdateEventGameUserAnswer(EventGameUserAnswer userAnswerToUpdate);
        void DeleleEventGameUserAnswer(EventGameUserAnswer userAnswerToDelete);
    }
}

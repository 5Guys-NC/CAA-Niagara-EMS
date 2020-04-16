using CAA_Event_Management.Models;
/******************************
*  Model Created By: Max Cashmore
*******************************/
namespace CAA_Event_Management.ViewModels
{
    public class ResultsViewModel
    {
        public int QuestionCount { get; set; }
        public int CorrectAnswerCount { get; set; }
        public Event Event { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CAA_Event_Management.Models;

namespace CAA_Event_Management.ViewModels
{
    public class ResultsViewModel
    {
        public int QuestionCount { get; set; }
        public int CorrectAnswerCount { get; set; }
        public Event Event { get; set; }
    }
}

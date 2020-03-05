using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    public class GameModel
    {
        public int ID { get; set; }
        //holds question
        public string QuestionText { get; set; }
        //the possible answers to select from
        public string OptionsText { get; set; }
        //text of answer(s) that is correct 
        public string AnswerText { get; set; }

        public int GameID { get; set; }
        public Game Game { get; set; }
    }
}

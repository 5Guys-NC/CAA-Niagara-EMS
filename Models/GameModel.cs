/*************************
 * Created By: Max Cashmore
 * **********************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Object that stores a game's question(s) with each details stored in a '|' seperated string to save into the database.
    /// Details include: options to question, the correct answer to question, and answer's image (if any).
    /// </summary>
    public class GameModel
    {
        public int ID { get; set; }

        //holds question
        public string QuestionText { get; set; }

        public string QuestionImageId { get; set; }
        //the possible answers to select from
        
        public string OptionsText { get; set; }
        //text of answer(s) that is correct 
        
        public string AnswerText { get; set; }

        public string ImageIDs { get; set; }
        public int GameID { get; set; }
        public Game Game { get; set; }
    }
}

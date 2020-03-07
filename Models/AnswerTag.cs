/*************************
 * Created By: Max Cashmore
 * **********************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for AnswerTag table
    /// </summary>
    public class AnswerTag
    {
        public int ID { get; set; }

        public int AnswerID { get; set; }
        public Answer Answer { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}

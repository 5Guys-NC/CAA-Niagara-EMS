/*************************
 * Created By: Max Cashmore
 * **********************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for QuestionTag table
    /// </summary>
    public class QuestionTag
    {
        public int ID { get; set; }

        public int QuestionID { get; set; }
        public Question Question { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}

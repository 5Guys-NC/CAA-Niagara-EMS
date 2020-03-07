/********************
 * Created By: Max Cashmore
 * ******************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for AnswerPicture table
    /// </summary>
    public class AnswerPicture
    {
        public int ID { get; set; }

        public int AnswerID { get; set; }
        public Answer Answer { get; set; }

        public int PictureID { get; set; }
        public Picture Picture { get; set; }
    }
}


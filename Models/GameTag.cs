/*************************
 * Created By: Max Cashmore
 * **********************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for GameTag Table
    /// </summary>
    public class GameTag
    {
        public int ID { get; set; }

        public int GameID { get; set; }
        public Game Game { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}

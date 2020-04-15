using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/*************************
 * Created By: Jon Yade and Max Cashmore
 * **********************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for EventGameUserAnswer table
    /// </summary>
    public class EventGameUserAnswer
    {
        #region Table Fields

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string ID { get; set; }

        [Required]
        public bool answerWasCorrect { get; set; }

        #endregion

        #region Table Connections

        public int? answerID { get; set; }

        [StringLength(36)]
        public string AttendantID { get; set; }

        [Required(ErrorMessage = "Event ID Required")]
        [StringLength(36)]
        public string EventID { get; set; }
        public virtual Event Event { get; set; }

        [Required]
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }

        #endregion
    }
}

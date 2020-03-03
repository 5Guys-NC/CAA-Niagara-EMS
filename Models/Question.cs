using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/******************************
*  Model Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the QUESTION table in the database
    /// </summary>
    public class Question
    {
        public Question()
        {
            this.Answers = new HashSet<Answer>();
        }

        #region Table Connections

        public int ID { get; set; }

        [Display(Name = "Question")]
        public string Phrase { get; set; }

        //keywords as thought of by jon,
        //contains generic words that may help in searching
        public string Keywords { get; set; }

        //increase every time someone answers
        public int TotalFeedback { get; set; } = 0;

        //increase when someone answers correctly
        //correct/total = percent
        public int CorrectFeedback { get; set; } = 0;
        #endregion

        #region Audit Fields
        
        [Display(Name = "Created By")]
        [StringLength(250, ErrorMessage = "Created By cannot be more then 250 characters")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Last Modified By")]
        [StringLength(75, ErrorMessage = "Last Modified By cannot be more then 75 characters")]
        public string LastModifiedBy { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime? LastModifiedDate { get; set; }

        #endregion

        #region Table Connections
        public virtual ICollection<Answer> Answers { get; set; }
        public int GameID { get; set; }
        public virtual Game Game { get; set; }
        #endregion
    }
}

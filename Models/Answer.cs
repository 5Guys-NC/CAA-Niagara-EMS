using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/******************************
*  Model Created By: Max Cashmore
*  Edited By: Brian Culp
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the ANSWER table in the database
    /// </summary>
    public class Answer
    {
        #region Table Fields
        public int ID { get; set; }

        [Display(Name = "Answer")]
        [StringLength(50, ErrorMessage = "Answer cannot be more then 50 characters")]
        public string Phrase { get; set; }

        public bool? IsCorrect { get; set; } = false;
        #endregion

        public string Keywords { get; set; }

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
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }
        #endregion
    }
}

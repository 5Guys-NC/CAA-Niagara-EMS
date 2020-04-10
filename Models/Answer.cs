using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public Answer()
        {
            this.AnswerPictures = new HashSet<AnswerPicture>();
        }

        public int ID { get; set; }

        [Display(Name = "Answer")]
        [StringLength(50, ErrorMessage = "Answer cannot be more then 50 characters")]
        public string Text { get; set; }

        public int TimesUsed { get; set; }

        #region Table Fields

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Last Modified By")]
        public string LastModifiedBy { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<AnswerPicture> AnswerPictures { get; set; }

        #endregion
    }
}

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
    public class Answer : Auditable
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

        public virtual ICollection<AnswerPicture> AnswerPictures { get; set; }
    }
}

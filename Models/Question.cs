using System;
using System.ComponentModel.DataAnnotations;
/******************************
*  Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the QUESTION table in the database
    /// </summary>
    public class Question : Auditable
    {
        public int ID { get; set; }

        [Display(Name = "Question")]
        public string Text { get; set; }
        public int TimesUsed { get; set; }
    }
}

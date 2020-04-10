using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
/*****************************
 * Created By: Max Cashmore
 * ***************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for Picture Table
    /// </summary>
    public class Picture
    {
        public Picture()
        {
            this.AnswerPictures = new HashSet<AnswerPicture>();
        }

        public int ID { get; set; }
        public byte[] Image { get; set; }
        public string ImageFileName { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Last Modified By")]
        public string LastModifiedBy { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime? LastModifiedDate { get; set; }

        public ICollection<AnswerPicture> AnswerPictures { get; set; }
    }
}

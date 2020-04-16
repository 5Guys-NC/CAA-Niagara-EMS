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
    public class Picture : Auditable
    {
        public Picture()
        {
            this.AnswerPictures = new HashSet<AnswerPicture>();
        }

        public int ID { get; set; }
        public byte[] Image { get; set; }
        public string ImageFileName { get; set; }

        public ICollection<AnswerPicture> AnswerPictures { get; set; }
    }
}

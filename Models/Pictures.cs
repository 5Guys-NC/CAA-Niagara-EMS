using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/******************************
*  Models Created By: Brian Culp
*  Edited by: 
*******************************/
namespace CAA_Event_Management.Models
{
    class Pictures
    {
        public byte[] imageContent { get; set; }

        [StringLength(256)]
        public string imageMimeType { get; set; }

        [StringLength(100, ErrorMessage = "The name of the file cannot be more than 100 characters.")]
        [Display(Name = "File Name")]
        public string imageFileName { get; set; }
    }
}

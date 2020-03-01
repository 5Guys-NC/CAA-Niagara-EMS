using System.ComponentModel.DataAnnotations;
/******************************
*  Created By: Brian Culp
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

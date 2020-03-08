using System;
using System.ComponentModel.DataAnnotations;
/*************************
 * Created By: Max Cashmore
 * **********************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for Tag table
    /// </summary>
    public class Tag
    {
        public int ID { get; set; }
        public string Name { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Last Modified By")]
        public string LastModifiedBy { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime? LastModifiedDate { get; set; }
    }
}

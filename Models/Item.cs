using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
/******************************
*  Created By: Brian Culp
*  Edited by: Jon Yade
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the ITEMS table in the database
    /// </summary>
    public class Item : Auditable
    {
        public Item()
        {
            this.EventItems = new HashSet<EventItem>();
        }

        #region Table Fields

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string ItemID { get; set; } = "0";

        public string SummaryDisplay
        {
            get
            {
                return "Answer type: " + ValueType;
            }
        }

        [Display(Name = "Item")]
        [Required(ErrorMessage = "ItemName Required")]
        [StringLength(75,ErrorMessage ="Item Name cannot be more then 75 characters")]
        public string ItemName { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "valueType Required")]
        [StringLength(25, ErrorMessage = "valueType cannot be more then 25 characters")]
        public string ValueType { get; set; }


        public int? ItemCount { get; set; } = 0;

        [Display(Name = "isDeleted?")]
        public bool? IsDeleted { get; set; } = false;

        #endregion

        #region Table Connections

        public ICollection<EventItem> EventItems { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/******************************
*  Model Created By: Brian Culp
*  Edited by: Jon Yade
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the EVENTITEMS table in the database
    /// </summary>
    public class EventItem : Auditable
    {
        public EventItem()
        {
            this.AttendanceItems = new HashSet<AttendanceItem>();
        }

        #region Table Fields

        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string EventItemID { get; set; } = "0";

        #endregion

        #region Table Connections

        [Display(Name = "Event ID")]
        [Required(ErrorMessage = "Event ID Required")]
        [StringLength(36)]
        public string EventID { get; set; }
        public virtual Event Event { get; set; }
        //public ICollection<Event> Events { get; set; }

        [Display(Name = "Item ID")]
        [Required(ErrorMessage = "Item ID Required")]
        [StringLength(36)]
        public string ItemID { get; set; }
        public virtual Item Item { get; set; }
        //public ICollection<Item> Items {get; set;}

        public ICollection<AttendanceItem> AttendanceItems { get; set; }

        #endregion
    }
}

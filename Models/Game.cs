using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/******************************
*  Model Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Models
{
    public class Game
    {
        public Game()
        {
            this.Questions = new HashSet<Question>();
            this.CreatedDate = DateTime.Now;
        }

        #region Table Fields
        public int ID { get; set; }
        public string Title { get; set; }

        public virtual Event Event { get; set; }

        public string Keywords { get; set; }

        #endregion

        #region Audit Fields

        [Display(Name = "Created By")]
        [StringLength(250, ErrorMessage = "Created By cannot be more then 250 characters")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Last Modified By")]
        [StringLength(75, ErrorMessage = "Last Modified By cannot be more then 75 characters")]
        public string LastModifiedBy { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime? LastModifiedDate { get; set; }

        #endregion

        #region Table Connections
        public virtual ICollection<Question> Questions { get; set; }
        #endregion
    }
}

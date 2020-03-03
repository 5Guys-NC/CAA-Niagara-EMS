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
                GameModels = new HashSet<GameModel>();
            }

            #region Table Fields
            public int ID { get; set; }
            public string Title { get; set; }
            //public virtual Event Event { get; set; }

            public ICollection<GameModel> GameModels { get; set; }

            #endregion

            #region Audit Fields

            [Display(Name = "Created By")]
            public string CreatedBy { get; set; }

            [Display(Name = "Date Created")]
            public DateTime? CreatedDate { get; set; }

            [Display(Name = "Last Modified By")]
            public string LastModifiedBy { get; set; }

            [Display(Name = "Last Modified Date")]
            public DateTime? LastModifiedDate { get; set; }

            #endregion
        }
    }

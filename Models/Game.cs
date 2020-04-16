using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
/******************************
*  Created By: Max Cashmore
*  Edited by: Brian Culp
*******************************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    ///  Model for Game Table
    /// </summary>
    public class Game : Auditable
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
        }
    }

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    public class EventGameUserAnswer
    {
        #region Table Fields

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string ID { get; set; }

        [Required]
        public bool answerWasCorrect { get; set; }

        #endregion

        #region Table Connections

        public int? answerID { get; set; }

        [StringLength(36)]
        public string AttendantID { get; set; }

        [Required(ErrorMessage = "Event ID Required")]
        [StringLength(36)]
        public string EventID { get; set; }
        public virtual Event Event { get; set; }

        [Required]
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }

        #endregion
    }
}

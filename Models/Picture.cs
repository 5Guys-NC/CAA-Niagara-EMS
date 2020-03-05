﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    public class Picture
    {
        public int ID { get; set; }
        public byte[] Image { get; set; }
        public string ImageFileName { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "Last Modified By")]
        public string LastModifiedBy { get; set; }

        [Display(Name = "Last Modified Date")]
        public DateTime? LastModifiedDate { get; set; }

        public ICollection<AnswerPicture> answerPictures { get; set; }
    }
}
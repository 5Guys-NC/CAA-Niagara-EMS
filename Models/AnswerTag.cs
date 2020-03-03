using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    public class AnswerTag
    {
        public int ID { get; set; }

        public int AnswerID { get; set; }
        public Answer Answer { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}

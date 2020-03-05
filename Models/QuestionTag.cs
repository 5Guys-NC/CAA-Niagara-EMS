using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    public class QuestionTag
    {
        public int ID { get; set; }

        public int QuestionID { get; set; }
        public Question Question { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}

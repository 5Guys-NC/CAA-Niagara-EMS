using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    public class AnswerPicture
    {
        public int ID { get; set; }

        public int AnswerID { get; set; }
        public Answer Answer { get; set; }

        public int PictureID { get; set; }
        public Picture Picture { get; set; }
    }
}


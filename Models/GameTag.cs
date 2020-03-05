using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    public class GameTag
    {
        public int ID { get; set; }

        public int GameID { get; set; }
        public Game Game { get; set; }

        public int TagID { get; set; }
        public Tag Tag { get; set; }
    }
}

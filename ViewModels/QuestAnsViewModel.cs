using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Utilities
{/// <summary>
/// Used to work with the Game Model
/// </summary>
    public class QuestAnsViewModel
    {
        public QuestAnsViewModel()
        { this.IsTrue = false; }

        public string Text { get; set; }
        public bool IsTrue { get; set; }
        public int Index { get; set; }
    }
}

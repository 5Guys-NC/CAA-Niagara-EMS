using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/******************************
*  Repository Created By: Jon Yade
*  Edited by:
*******************************/
namespace CAA_Event_Management.Data
{
    public class EventItemDetails
    {
        internal int ID { get; set; }
        internal string EIDEventItemID { get; set; }
        internal string EIDEventID { get; set; }
        public string EIDEventDisplayName { get; set; }
        internal string EIDItemID { get; set; }
        public string EIDItemPhrase { get; set; }
        internal string EIDDataType { get; set; }
        internal int? EIDquestionCount { get; set; }
        internal bool? EIDItemAssigned { get; set; } = false;
    }
}

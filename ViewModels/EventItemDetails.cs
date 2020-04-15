/******************************
*  Created By: Jon Yade
*******************************/

namespace CAA_Event_Management.ViewModels
{
    /// <summary>
    /// View Model for EventItems Details
    /// </summary>
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

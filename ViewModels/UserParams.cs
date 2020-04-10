using CAA_Event_Management.Models;
/*********************************
 * Created By: Brian Culp
 * ******************************/

namespace CAA_Event_Management.ViewModels
{
    /// <summary>
    /// View Model to record extra parameters for UserAccount
    /// </summary>
    public class UserParams
    {
        internal UserAccount selectedUser { get; set; }
        internal bool editMode { get; set; }
    }
}

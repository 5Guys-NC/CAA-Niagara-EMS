using CAA_Event_Management.Models;
/*********************************
 * Created By: Brian Culp
 * ******************************/

namespace CAA_Event_Management.ViewModels
{
    /// <summary>
    /// View Model to record extra parameters for User
    /// </summary>
    public class UserParams
    {
        internal Users selectedUser { get; set; }
        internal bool editMode { get; set; }
    }
}

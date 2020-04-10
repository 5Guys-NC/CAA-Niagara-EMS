using System;
/******************************
 * Created By: Jon Yade
 * ***************************/

namespace CAA_Event_Management.Models
{
    /// <summary>
    /// The page interface for IAuditable
    /// </summary>
    internal interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedDate { get; set; }
    }
}

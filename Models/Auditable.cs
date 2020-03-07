using System;
using System.ComponentModel.DataAnnotations;
/********************************
 * Created By: Jon Yade
 * *****************************/

namespace CAA_Event_Management.Models
{
    /// <summary>
    /// This page is a model to template the Auditable table in the database
    /// </summary>
    public abstract class Auditable : IAuditable
    {
        [ScaffoldColumn(false)]
        [StringLength(256)]
        public string CreatedBy { get; set; } = "CAA Employee";

        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [ScaffoldColumn(false)]
        [StringLength(256)]
        public string LastModifiedBy { get; set; } = "CAA Emplyee";

        [ScaffoldColumn(false)]
        public DateTime? LastModifiedDate { get; set; } = DateTime.Now;


        //This can possible be uncommented for inclusion later for concurrency
        //[ScaffoldColumn(false)]
        //[Timestamp]
        //public Byte[] RowVersion { get; set; }
    }
}

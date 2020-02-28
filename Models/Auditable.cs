using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    public abstract class Auditable : IAuditable
    {
        [ScaffoldColumn(false)]
        [StringLength(250)] //maybe make this 256 later
        public string CreatedBy { get; set; } = "CAA Employee";

        [ScaffoldColumn(false)]
        public DateTime? CreatedDate { get; set; } = DateTime.Today;

        [ScaffoldColumn(false)]
        [StringLength(75)]  //maybe make this 256 later
        public string LastModifiedBy { get; set; } = "CAA Emplyee";

        [ScaffoldColumn(false)]
        public DateTime? LastModifiedDate { get; set; } = DateTime.Today;


        //This can possible be uncommented for inclusion later for concurrency
        //[ScaffoldColumn(false)]
        //[Timestamp]
        //public Byte[] RowVersion { get; set; }
    }
}

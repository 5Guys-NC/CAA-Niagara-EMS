using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CAA_Event_Management.Models
{
    internal interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTime? CreatedDate { get; set; }
        string LastModifiedBy { get; set; }
        DateTime? LastModifiedDate { get; set; }
    }
}

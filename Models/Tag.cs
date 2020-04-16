using System;
using System.ComponentModel.DataAnnotations;
/*************************
 * Created By: Max Cashmore
 * **********************/
namespace CAA_Event_Management.Models
{
    /// <summary>
    /// Model for Tag table
    /// </summary>
    public class Tag : Auditable
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}

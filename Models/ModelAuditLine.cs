using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAA_Event_Management.Models
{
    public class ModelAuditLine
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [StringLength(36)]
        public string ID { get; set; }

        [Required]
        [StringLength(50)]
        public string AuditorName { get; set; }

        [Required]
        [StringLength(50)]
        public string ObjectTable { get; set; }

        [Required]
        [StringLength(36)]
        public string ObjectID { get; set; }

        [Required]
        [StringLength(5000)]
        public string NewObjectInfo { get; set; }

        [Required]
        [StringLength(30)]
        public string TypeOfChange { get; set; }

        [Required]
        [StringLength(30)]
        public string DateTimeOfChange { get; set; }

        [Required]
        [StringLength(5000)]
        public string ChangedFieldValues { get; set; }

    }
}

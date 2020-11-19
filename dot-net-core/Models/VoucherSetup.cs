using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class VoucherSetup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Terms { get; set; }
        public string Limitations { get; set; }
        public int Type { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }
        public bool IsMultipleTimeUsage { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class Emenu_MenuSchedules
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; }
        public int MenuId { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string UniqueCode { get; set; }
        public int ConceptId { get; set; }
        public int StoreId { get; set; }
    }
}

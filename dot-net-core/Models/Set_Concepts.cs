using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class Set_Concepts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ConceptName { get; set; }
        public DateTime? CreatedOn { get; set; }

        [DefaultValue(0)]
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        [DefaultValue(0)]
        public int? ModifiedBy { get; set; }
        public byte? Status { get; set; }

        [DefaultValue(false)]
        public bool Active { get; set; }
        public DateTime? DeletedOn { get; set; }
        public int DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ClientId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class Concept_Theme
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ConceptId { get; set; }
        public string ColorCode { get; set; }
        public string LogoName { get; set; }
        public string FeedBackIconName { get; set; }
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

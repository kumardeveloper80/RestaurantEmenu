using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class CRM_CQuestions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int q_id { get; set; }
        public string q_question { get; set; }
        public string q_shortdescription { get; set; }
        public string q_managerperformanace { get; set; }
        public int? cqg_id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modifiedon { get; set; }
        public string ModifiedBy { get; set; }
        public short? q_status { get; set; }
        public bool? active { get; set; } = true;
    }
}

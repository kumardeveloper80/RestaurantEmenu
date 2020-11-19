using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class CRM_CQuestionsGroups
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cqg_id { get; set; }
        public string cqg_name { get; set; }
        public string cqg_description { get; set; }
        public string Createdby { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? cqg_status { get; set; }
        public bool? active { get; set; } = true;
        public bool? ServerPerformance { get; set; } = false;
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class CRM_CommentCardResults
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ctr_id { get; set; }
        public int? CCR_ID { get; set; }
        public int? q_id { get; set; }
        public string ctr_answer { get; set; }
        public string ctr_AnswerText { get; set; }
        public string ctr_score { get; set; }
        public string q_type { get; set; }
        public string weight { get; set; }
        public string Calculated { get; set; }
        public string LineBased { get; set; }
        public string Required { get; set; }
        public string Answered { get; set; }
    }
}

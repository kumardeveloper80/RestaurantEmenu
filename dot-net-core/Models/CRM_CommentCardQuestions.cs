using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class CRM_CommentCardQuestions
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ccq_id { get; set; }
        public int cc_id { get; set; }
        public int q_id { get; set; }
        public int? q_ordre { get; set; }
        public string q_type { get; set; }
        public string q_typevalue { get; set; } = "";
        public string q_answerDisplay { get; set; }
        public int? q_weight { get; set; }
        public bool? q_calculated { get; set; } = true;
        public int? q_minval { get; set; }
        public bool? q_required { get; set; } = false;
        public bool? q_linebased { get; set; }
        public int? cqg_order { get; set; }
        public bool? cqg_visible { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string Modifiedby { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? status { get; set; }
        public bool? q_bootstrapSkin { get; set; } = true;
        public bool? ShowQuestionText { get; set; } = true;
        public string textPlaceholder { get; set; }
        public bool? AnswerFullSpace { get; set; } = false;
        public int? Relatedquestion { get; set; }
    }
}

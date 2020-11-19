using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Models
{
    public class CRM_CommentCard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int cc_id { get; set; }
        public string cc_name { get; set; }
        public string cc_description { get; set; }
        public bool? linebased { get; set; } = false;
        public bool? AvgScore { get; set; } = true;
        public int? cc_minvalue { get; set; }
        public string Display { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? ct_status { get; set; }
        public bool? active { get; set; } = true;
        public bool? Notify_Bymail { get; set; } = true;
        public bool? Notify_Bysms { get; set; } = false;
        public string Title { get; set; }
        public bool? ColorCoded { get; set; } = true;
        public string CCheaderIMG { get; set; }
        public string CCfooterIMG { get; set; }
        public string PRheaderIMG { get; set; }
        public string PRfooterIMG { get; set; }
        public string CCheaderHeight { get; set; }
        public string CCfooterHeight { get; set; }
        public string PRheaderHeight { get; set; }
        public string PRfooterHeight { get; set; }
        public string CCControlsBorderColor { get; set; }
        public string PRControlsBorderColor { get; set; }
        public string CCFontFamily { get; set; }
        public string PRFontFamily { get; set; }
        public string CCFontColor { get; set; }
        public string PRFontColor { get; set; }
        public string CCFontSize { get; set; }
        public string PRFontSize { get; set; }
        public string PRBorderWidth { get; set; }
        public string CCBorderWidth { get; set; }
        public string PRFontStyle { get; set; }
        public string CCFontStyle { get; set; }
        public string PRFontWeight { get; set; }
        public string CCFontWeight { get; set; }
        public bool? CCCustomFont { get; set; } = false;
        public string CCCustomFontFamily { get; set; }
        public string CCCustomFontLocal { get; set; }
        public string CCCustomFontFile { get; set; }
        public string CCCustomFontFormat { get; set; }
        public string ThankYouImg { get; set; }
        public string CCType { get; set; } = "H";
        public string ProfileLeftImg { get; set; }
        public string ProfileRightImg { get; set; }
        public string CCLeftImg { get; set; }
        public string CCRightImg { get; set; }
        public string CCBackground { get; set; }
        public string ProfileTitle { get; set; }
        public string ProfileTitle1 { get; set; }
        public string SubmitbtnBackColor { get; set; }
        public string SubmitbtnForeColor { get; set; }
    }
}

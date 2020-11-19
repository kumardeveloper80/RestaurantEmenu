using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMenuApplication.Models
{
    public class Sys_Fields
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string FieldID { get; set; }
        public string FormID { get; set; }
        public string DataType { get; set; }
        public int? ModuleID { get; set; }
        public string SectionID { get; set; }
        public bool? Required { get; set; }
        public bool? Visible { get; set; }
        public string Label { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public int? Col { get; set; }
        public int? MaxLength { get; set; }
        public string TextMode { get; set; }
        public string SourceTable { get; set; }
        public string SourceValue { get; set; }
        public string SourceText { get; set; }
        public bool? hasActive { get; set; }
        public string sourcePageURL { get; set; }
        public string RequiredMessage { get; set; }
        public string RequiredPatternMessage { get; set; }
        public string RequiredPattern { get; set; }
        public bool? Readonly { get; set; }
        public int? Position { get; set; }
        public string Skin { get; set; }
        public string CSS { get; set; }
        public bool? CustomField { get; set; }
        public bool? Incomplete { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool? Active { get; set; }
    }
}

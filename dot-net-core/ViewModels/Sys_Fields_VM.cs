using System.Collections.Generic;

namespace EMenuApplication.ViewModels
{
    public class Sys_Fields_VM
    {
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
        public bool? Active { get; set; }
        public string UserAnswer { get; set; }
        public List<Options_VM> Options { get; set; }
        public Sys_Fields_VM()
        {
            Options = new List<Options_VM>();
        }
    }
}

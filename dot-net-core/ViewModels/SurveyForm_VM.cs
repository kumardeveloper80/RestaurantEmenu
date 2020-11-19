using EMenuApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class SurveyForm_VM
    {
        public Guid Store_ID { get; set; }
        public int Storeno { get; set; }
        public CommentCard_VM CommentCard { get; set; }
        public List<Sys_Fields_VM> SysFields { get; set; }
        public List<QuestionMaster_VM> QuestionMaster { get; set; }

        public SurveyForm_VM()
        {
            SysFields = new List<Sys_Fields_VM>();
            QuestionMaster = new List<QuestionMaster_VM>();
        }
    }
    public class CommentCard_VM
    {
        public int cc_id { get; set; }
        public string cc_name { get; set; }
        public string cc_description { get; set; }
        public bool? linebased { get; set; } = false;
        public bool? AvgScore { get; set; } = true;
        public int? cc_minvalue { get; set; }
        public string Display { get; set; }
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
    public class QuestionMaster_VM
    {
       
        public List<Question_VM> Questions { get; set; }
        public string QuestionGroup { get; set; }
        public string QuestionGroupDesc { get; set; }
        public QuestionMaster_VM()
        {
            Questions = new List<Question_VM>();
        }
    }
    public class CommentCardQuestions_VM
    {
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
        public short? status { get; set; }
        public bool? q_bootstrapSkin { get; set; } = true;
        public bool? ShowQuestionText { get; set; } = true;
        public string textPlaceholder { get; set; }
        public bool? AnswerFullSpace { get; set; } = false;
        public int? Relatedquestion { get; set; }
    }
    public class CQuestions_VM
    {
        public int q_id { get; set; }
        public string q_question { get; set; }
        public string q_shortdescription { get; set; }
        public string q_managerperformanace { get; set; }
        public int? cqg_id { get; set; }
        public short? q_status { get; set; }
        public bool? active { get; set; } = true;
    }
    public class CQuestionsGroups_VM
    {
        public int cqg_id { get; set; }
        public string cqg_name { get; set; }
        public string cqg_description { get; set; }
        public short? cqg_status { get; set; }
        public bool? active { get; set; } = true;
        public bool? ServerPerformance { get; set; } = false;
    }
    public class Question_VM
    {
        public int q_id { get; set; }
        public string q_question { get; set; }
        public string q_shortdescription { get; set; }
        public int? q_order { get; set; }
        public string q_type { get; set; }
        public string q_typevalue { get; set; } = "";
        public bool? q_required { get; set; } = false;
        public string  questionGroup { get; set; }
        public string questionGroupDesc { get; set; }
        public string userAnswer { get; set; }
        public List<Options_VM> options { get; set; }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public Question_VM()
        {
            options = new List<Options_VM>();
        }
    }
}

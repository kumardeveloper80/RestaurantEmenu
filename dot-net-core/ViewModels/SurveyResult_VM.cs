using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.ViewModels
{
    public class SurveyResult_VM
    {
        public int CCR_ID { get; set; }
        public Guid? Customer_ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public string NationalityName { get; set; }
        public string CORName { get; set; }
        public List<SurveyResultDeatils_VM> SurveyResultDeatils { get; set; }
        public SurveyResult_VM()
        {
            SurveyResultDeatils = new List<SurveyResultDeatils_VM>();
        }
    }

    public class SurveyResultDeatils_VM
    {
        public string Question { get; set; }
        public string Answer { get; set; }
        public string AnswerText { get; set; }
    }
}

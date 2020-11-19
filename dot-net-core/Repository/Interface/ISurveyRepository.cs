using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;

namespace EMenuApplication.Repository.Interface
{
    public interface ISurveyRepository
    {
        SurveyForm_VM GetSurveyFormByStore(Guid storeid);
        int Save(SurveyForm_VM surveyForm);
        List<SurveyResult_VM> GetFillUpSurvey(int loginUserId, Guid storeid);
        SurveyResult_VM GetFillUpSurveyDetails(int loginUserId, Guid storeid, int id);
    }
}

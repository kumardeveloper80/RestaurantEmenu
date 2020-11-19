using AutoMapper;
using EMenuApplication.Models;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EMenuApplication.Repository
{
    public class SurveyRepository : ISurveyRepository
    {
        /// <summary>
        /// read only properties
        /// </summary>
        private readonly EMenuDBContext _context;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor to inject various services and context
        /// </summary>
        /// <param name="context"></param>
        public SurveyRepository(EMenuDBContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Get survey form by store-id
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
        [Obsolete]
        public SurveyForm_VM GetSurveyFormByStore(Guid storeGuid)
        {
            try
            {
                var result = (from ccs in _context.CRM_CommentCardStores
                              join cc in _context.CRM_CommentCard on ccs.cc_id equals cc.cc_id
                              where ccs.Store_ID == storeGuid && cc.active.Value && ccs.active.Value
                              select new SurveyForm_VM
                              {
                                  Store_ID = ccs.Store_ID,
                                  Storeno = ccs.ID,
                                  CommentCard = _mapper.Map<CommentCard_VM>(cc),
                              }).FirstOrDefault();

                if (result != null)
                {
                    result.SysFields = _context.Sys_Fields
                        .Where(x => x.FormID == Helper.CommentCardResultHeader && x.Active.Value)
                        .OrderBy(x => x.Position)
                        .Select(x => _mapper.Map<Sys_Fields_VM>(x)).ToList();

                    // Fields value option for radiobutton, checkbox, dropdown
                    foreach (var sysField in result.SysFields)
                    {
                        // For dropdwon
                        if (sysField.Type == HTMLELEMENT.DROPDOWN.ToString() && (sysField.FieldID == Helper.COR || sysField.FieldID == Helper.Nationality))
                        {
                            sysField.Options = _context.Set_CountryCodes.Select(x => new Options_VM()
                            {
                                Text = x.Name,
                                Value = x.CountryID.ToString()
                            }).ToList();
                        }
                        else
                        {
                            sysField.Options = _context.Sys_FieldValues
                               .Where(x => x.FormID == Helper.CommentCardResultHeader && x.FieldID == sysField.FieldID)
                               .OrderBy(x => x.Position)
                               .Select(x => new Options_VM()
                               {
                                   Text = x.Text,
                                   Value = x.Value
                               }).ToList();
                        }
                    }

                    // Questions
                    result.QuestionMaster = (from ccq in _context.CRM_CommentCardQuestions
                                             join cq in _context.CRM_CQuestions on ccq.q_id equals cq.q_id
                                             join cqg in _context.CRM_CQuestionsGroups on cq.cqg_id equals cqg.cqg_id
                                             where ccq.cc_id == result.CommentCard.cc_id
                                             select new Question_VM
                                             {
                                                 q_id = cq.q_id,
                                                 q_question = cq.q_question,
                                                 q_shortdescription = cq.q_shortdescription,
                                                 q_order = ccq.q_ordre,
                                                 q_type = ccq.q_type,
                                                 q_typevalue = ccq.q_typevalue,
                                                 q_required = ccq.q_required,
                                                 questionGroup = cqg.cqg_name,
                                                 questionGroupDesc = cqg.cqg_description,
                                                 options = (from setDrp in _context.set_DropDown
                                                            where setDrp.DD_ID == ccq.q_typevalue && setDrp.status.Value == 1
                                                            orderby setDrp.DD_ValueOrder
                                                            select new Options_VM()
                                                            {
                                                                Text = setDrp.DD_Display,
                                                                Value = setDrp.DD_Value,
                                                            }).ToList()
                                             }).ToList()
                                           .GroupBy(x => x.questionGroup)
                                           .Select(y => new QuestionMaster_VM()
                                           {
                                               QuestionGroup = y.Key.ToString(),
                                               QuestionGroupDesc = y.Select(x => x.questionGroupDesc).FirstOrDefault(),
                                               Questions = y.ToList()
                                           }).ToList();

                    // For Slider min and max value
                    foreach (var qm in result.QuestionMaster)
                    {
                        foreach (var que in qm.Questions)
                        {
                            if (que.q_type == HTMLELEMENT.SLIDER.ToString())
                            {
                                que.MaxValue = que.options.Where(x => x.Value != null).Select(x => int.Parse(x.Value)).ToList().Max();
                                que.MinValue = que.options.Where(x => x.Value != null).Select(x => int.Parse(x.Value)).ToList().Min();
                            }
                        }
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Save fill-up survey
        /// </summary>
        /// <param name="surveyForm"></param>
        /// <returns></returns>
        public int Save(SurveyForm_VM surveyForm)
        {
            int ret = 0;
            using (var dbcxtransaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Save user's info
                    var obj = new CRM_CommentCardResultsHeader();
                    obj.cc_id = surveyForm.CommentCard.cc_id;
                    obj.Customer_ID = Guid.NewGuid();
                    obj.Store_ID = surveyForm.Store_ID;
                    obj.Storeno = surveyForm.Storeno;

                    foreach (var user in surveyForm.SysFields)
                    {
                        if (user.FieldID == USERINFO.FirstName.ToString())
                        {
                            obj.FirstName = user.UserAnswer;
                        }
                        else if (user.FieldID == USERINFO.LastName.ToString())
                        {
                            obj.LastName = user.UserAnswer;
                        }
                        else if (user.FieldID == USERINFO.Email.ToString())
                        {
                            obj.Email = user.UserAnswer;
                        }
                        else if (user.FieldID == USERINFO.Mobile.ToString())
                        {
                            obj.Mobile = user.UserAnswer;
                        }
                        else if (user.FieldID == USERINFO.Birthday.ToString() && user.UserAnswer != null)
                        {
                            obj.Birthday = DateTime.Parse(user.UserAnswer);
                        }
                        else if (user.FieldID == USERINFO.Gender.ToString())
                        {
                            obj.Gender = user.UserAnswer;
                        }
                        else if (user.FieldID == USERINFO.Nationality.ToString() && user.UserAnswer != null)
                        {
                            obj.Nationality = Convert.ToInt32(user.UserAnswer);
                        }
                        else if (user.FieldID == USERINFO.COR.ToString() && user.UserAnswer != null)
                        {
                            obj.COR = Convert.ToInt32(user.UserAnswer);
                        }
                    }
                    obj.CreatedOn = DateTime.Now;
                    _context.CRM_CommentCardResultsHeader.Add(obj);
                    _context.SaveChanges();


                    // Save the question's answer
                    var commentCardResults = new List<CRM_CommentCardResults>();
                    foreach (var qm in surveyForm.QuestionMaster)
                    {
                        foreach (var que in qm.Questions)
                        {
                            var answer = new CRM_CommentCardResults();
                            answer.CCR_ID = obj.CCR_ID;
                            answer.q_id = que.q_id;
                            answer.ctr_answer = que.userAnswer;
                            answer.ctr_AnswerText = que.options.Where(x => x.Value == que.userAnswer).Select(x => x.Text).FirstOrDefault();
                            answer.q_type = que.q_type;
                            commentCardResults.Add(answer);
                        }
                    }

                    _context.CRM_CommentCardResults.AddRange(commentCardResults);
                    _context.SaveChanges();
                    dbcxtransaction.Commit();
                    ret = 1;
                }
                catch (Exception ex)
                {
                    dbcxtransaction.Rollback();
                }
            }
            return ret;
        }

        /// <summary>
        /// Get Basic Info of Fill up survey
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="storeid"></param>
        public List<SurveyResult_VM> GetFillUpSurvey(int loginUserId, Guid storeid)
        {
            var result = _context.CRM_CommentCardResultsHeader.Where(x => x.Store_ID == storeid)
                .Select(x => new SurveyResult_VM()
                {
                    CCR_ID = x.CCR_ID,
                    Customer_ID = x.Customer_ID,
                    FirstName = x.FirstName != null ? x.FirstName : "",
                    LastName = x.LastName != null ? x.LastName : "",
                    Email = x.Email != null ? x.Email : "",
                    Mobile = x.Mobile != null ? x.Mobile : "",
                    Gender =x.Gender != null ? x.Gender : ""
                }).ToList();
            return result;
        }

        /// <summary>
        /// Get Fill Up Survey Details 
        /// </summary>
        /// <param name="loginUserId"></param>
        /// <param name="storeid"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public SurveyResult_VM GetFillUpSurveyDetails(int loginUserId, Guid storeid, int id)
        {
            var result = _context.CRM_CommentCardResultsHeader.Where(x => x.Store_ID == storeid && x.CCR_ID == id)
            .Select(x => new SurveyResult_VM()
            {
                FirstName = x.FirstName != null ? x.FirstName : "",
                LastName = x.LastName != null ? x.LastName : "",
                Email = x.Email != null ? x.Email : "",
                Mobile = x.Mobile != null ? x.Mobile : "",
                Gender = x.Gender != null ? x.Gender : "",
                Birthday = x.Birthday != null ? x.Birthday.Value.ToString("MM/dd/yyyy") : "",
                NationalityName = _context.Set_CountryCodes.Where(c => c.CountryID == x.Nationality.Value).Select(x => x.Name).FirstOrDefault(),
                CORName = _context.Set_CountryCodes.Where(c => c.CountryID == x.COR.Value).Select(x => x.Name).FirstOrDefault(),
                SurveyResultDeatils = _context.CRM_CommentCardResults.Where(c => c.CCR_ID == x.CCR_ID)
                .Select(z => new SurveyResultDeatils_VM()
                {
                    Answer = z.ctr_answer,
                    AnswerText = z.ctr_AnswerText,
                    Question = _context.CRM_CQuestions.Where(x => x.q_id == z.q_id.Value).Select(c => c.q_question).FirstOrDefault()
                }).ToList()
            }).FirstOrDefault();

            return result;
        }
    }
}

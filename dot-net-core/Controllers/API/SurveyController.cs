using System;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EMenuApplication.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class SurveyController : ControllerBase
    {
        ISurveyRepository _surveyRepository;
        public SurveyController(ISurveyRepository surveyRepository)
        {
            _surveyRepository = surveyRepository;
        }

        /// <summary>
        /// Function for get survey form
        /// </summary>
        /// <param name="storeid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{storeGuid}")]
        public IActionResult Get(Guid storeGuid)
        {
            CommonResponse<SurveyForm_VM> response = new CommonResponse<SurveyForm_VM>();
            var result = _surveyRepository.GetSurveyFormByStore(storeGuid);
            if (result != null)
            {
                response.status = Helper.success_code;
                response.dataenum = result;
            }
            else
            {
                response.message = Message.surveyNotFound;
            }
            return Ok(response);
        }

        /// <summary>
        /// Function for save fill-up survey
        /// </summary>
        /// <param name="surveyForm_VM"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Save([FromBody] SurveyForm_VM surveyForm)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            if(surveyForm != null)
            {
                response.status = _surveyRepository.Save(surveyForm);
                if(response.status == 1)
                {
                    response.message = Message.surveySave;
                }
                else
                {
                    response.message = Message.surveySaveError;
                }
            }
            else
            {
                response.message = Message.badRequest;
            }
            return Ok(response);
        }
    }
}

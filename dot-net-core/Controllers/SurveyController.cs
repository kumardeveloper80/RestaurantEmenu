using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace EMenuApplication.Controllers
{
    public class SurveyController : BaseController
    {
        private readonly IStoresRespository _storesRespository;
        private readonly ISurveyRepository _surveyRepository;
        public SurveyController(IStoresRespository storesRespository, ISurveyRepository surveyRepository,  IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _storesRespository = storesRespository;
            _surveyRepository = surveyRepository;
        }

        public IActionResult Index(Guid storeid, int id)
        {
            var result = _surveyRepository.GetFillUpSurveyDetails(this.loginUserId, storeid, id);
            return View(result);
        }
        public IActionResult List()
        {
            ViewBag.StoresList = new SelectList(_storesRespository.GetStoresByUserId(this.loginUserId), "StoreGuid", "StoreName");
            return View();
        }

        [HttpPost]
        public IActionResult GetList(DataTableAjaxPostModel param)
        {
            int recordsTotal = 0;
            var result = _surveyRepository.GetFillUpSurvey(this.loginUserId, param.storeGuid);
            if (result.Any())
            {
                recordsTotal = result.Count;
                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.FirstName.ToLower().Contains(search.ToLower())
                        || x.LastName.ToLower().Contains(search.ToLower())
                        || x.Gender.ToLower().Contains(search.ToLower())
                        || x.Email.ToLower().Contains(search.ToLower())
                        || x.Mobile.ToLower().Contains(search.ToLower())).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.FirstName).ToList() : result.OrderBy(p => p.FirstName).ToList();
                    break;
                case "1":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.LastName).ToList() : result.OrderBy(p => p.LastName).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Gender).ToList() : result.OrderBy(p => p.Gender).ToList();
                    break;
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Email).ToList() : result.OrderBy(p => p.Email).ToList();
                    break;
                case "4":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Mobile).ToList() : result.OrderBy(p => p.Mobile).ToList();
                    break;
                default:
                    result = result.OrderByDescending(p => p.CCR_ID).ToList();
                    break;
            }

            var data = result;
            if (param.length > 0)
            {
                data = result.Skip(param.start).Take(param.length).ToList();
            }

            return Json(new
            {
                draw = param.draw,
                recordsFiltered = result.Count,
                recordsTotal = recordsTotal,
                data = data
            });
        }
    }
}

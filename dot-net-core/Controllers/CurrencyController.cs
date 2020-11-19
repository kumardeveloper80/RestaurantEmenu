using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EMenuApplication.Controllers
{
    public class CurrencyController : BaseController
    {
        private readonly ICurrencyRepository _currencyRepository;
        public CurrencyController(ICurrencyRepository currencyRepository,  IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _currencyRepository = currencyRepository;
        }

        public IActionResult Index(int Id = 0)
        {
            var currency = new Currency_VM();
            if (Id > 0)
            {
                currency = _currencyRepository.Get(Id, loginUserId);
                if (currency == null)
                {
                    return RedirectToAction("List", "Currency");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(currency);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(currency);
            }
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetList(DataTableAjaxPostModel param)
        {
            int recordsTotal = 0;
            var result = _currencyRepository.GetList(loginUserId);

            if (result.Any())
            {
                recordsTotal = result.Count;
                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.Currency.ToLower().Contains(search.ToLower())).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Currency).ToList() : result.OrderBy(p => p.Currency).ToList();
                    break;
                default:
                    result = result.OrderByDescending(p => p.Id).ToList();
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

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            var result = _currencyRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.currencyDeletedError;
            }
            else if (result == Helper.refernce_error_code)
            {
                response.message = Message.referenceError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.currencyDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Currency_VM currency_VM)
        {
            if (currency_VM != null)
            {
                currency_VM.ClientId = clientId;
                if (currency_VM.Id > 0)
                {
                    if (_currencyRepository.Update(currency_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.currencyUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.currencyUpdateError;
                    }
                }
                else
                {
                    if (_currencyRepository.Add(currency_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.currencyAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.currencyAddedError;
                    }
                }
            }
            return RedirectToAction("List", "Currency");
        }

        [HttpGet]
        public IActionResult CheckCurrencyCode(int id, string currency)
        {
            return Ok(_currencyRepository.IsUniqueCurrencyCode(id, currency, loginUserId));
        }
    }
}

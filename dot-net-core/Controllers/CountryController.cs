using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EMenuApplication.Controllers
{
    public class CountryController : BaseController
    {
        private readonly ICountryRepository _countryRepository;
        public CountryController(ICountryRepository countryRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _countryRepository = countryRepository;
        }

        public IActionResult Index(int Id = 0)
        {
            var country = new Country_VM();
            if (Id > 0)
            {
                country = _countryRepository.Get(Id);
                if (country == null)
                {
                    return RedirectToAction("List", "Country");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(country);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(country);
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
            var result = _countryRepository.GetList();

            if (result.Any())
            {
                recordsTotal = result.Count;
                if (param.status == Helper.Active)
                {
                    result = result.Where(x => x.Status).ToList();
                }
                else if (param.status == Helper.Inactive)
                {
                    result = result.Where(x => !x.Status).ToList();
                }

                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.Name.ToLower().Contains(search.ToLower())
                    || x.Country.ToLower().Contains(search.ToLower())
                    || x.Code.ToLower().Contains(search.ToLower())
                    || x.timezone.ToLower().Contains(search.ToLower())
                    || Convert.ToString(x.Digits).Contains(search.ToLower())
                    || Convert.ToString(x.minDigits).Contains(search.ToLower())
                    ).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Name).ToList() : result.OrderBy(p => p.Name).ToList();
                    break;
                case "1":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Country).ToList() : result.OrderBy(p => p.Country).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Code).ToList() : result.OrderBy(p => p.Code).ToList();
                    break;
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Digits).ToList() : result.OrderBy(p => p.Digits).ToList();
                    break;
                case "4":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.minDigits).ToList() : result.OrderBy(p => p.minDigits).ToList();
                    break;
                case "5":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.timezone).ToList() : result.OrderBy(p => p.timezone).ToList();
                    break;
                default:
                    result = result.OrderByDescending(p => p.CountryID).ToList();
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
            var result = _countryRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.countryDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.countryDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Country_VM country_VM)
        {
            if (country_VM != null)
            {
                if (country_VM.CountryID > 0)
                {
                    if (_countryRepository.Update(country_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.countryUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.countryUpdateError;
                    }
                }
                else
                {
                    if (_countryRepository.Add(country_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.countryAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.countryAddedError;
                    }
                }
            }
            return RedirectToAction("List", "Country");
        }

        [HttpGet]
        public IActionResult CheckUniqueCountryName(int id, string countryName)
        {
            return Ok(_countryRepository.IsUniqueCountryName(id, countryName));
        }
    }
}

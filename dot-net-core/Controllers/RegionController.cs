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
    public class RegionController : BaseController
    {
        private readonly IRegionRepository _regionRepository;
        public RegionController(IRegionRepository regionRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _regionRepository = regionRepository;
        }

        public IActionResult Index(int Id = 0)
        {
            var region = new Region_VM();
            if (Id > 0)
            {
                region = _regionRepository.Get(Id);
                if (region == null)
                {
                    return RedirectToAction("List", "Region");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(region);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(region);
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
            var result = _regionRepository.GetList();

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
                    result = result.Where(x => x.Region.ToLower().Contains(search.ToLower())).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Region).ToList() : result.OrderBy(p => p.Region).ToList();
                    break;
                default:
                    result = result.OrderByDescending(p => p.RegionId).ToList();
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
            var result = _regionRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.regionDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.regionDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Region_VM region_VM)
        {
            if (region_VM != null)
            {
                if (region_VM.RegionId > 0)
                {
                    if (_regionRepository.Update(region_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.regionUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.regionUpdateError;
                    }
                }
                else
                {
                    if (_regionRepository.Add(region_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.regionAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.regionAddedError;
                    }
                }
            }
            return RedirectToAction("List", "Region");
        }

        [HttpGet]
        public IActionResult CheckUniqueRegionName(int id, string regionName)
        {
            return Ok(_regionRepository.IsUniqueRegionName(id, regionName));
        }
    }
}

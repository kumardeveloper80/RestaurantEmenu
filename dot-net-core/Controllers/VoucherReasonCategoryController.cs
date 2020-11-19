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
    public class VoucherReasonCategoryController : BaseController
    {
        private readonly IVoucherReasonCategoryMasterRepository _voucherReasonCategoryMasterRepository;
        public VoucherReasonCategoryController(
            IVoucherReasonCategoryMasterRepository voucherReasonCategoryMasterRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _voucherReasonCategoryMasterRepository = voucherReasonCategoryMasterRepository;

        }
        public IActionResult Index(int Id = 0)
        {
            var category = new ReasonCategoryMaster_VM();
            if (Id > 0)
            {
                category = _voucherReasonCategoryMasterRepository.Get(Id);
                if (category == null)
                {
                    return RedirectToAction("List", " VoucherReasonCategory");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(category);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(category);
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
            var result = _voucherReasonCategoryMasterRepository.GetList().Where(x => x.ClientId == clientId).ToList();

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
                    result = result.Where(x => x.Name.ToLower().Contains(search.ToLower())).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Name).ToList() : result.OrderBy(p => p.Name).ToList();
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
            var result = _voucherReasonCategoryMasterRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.voucherCategoryDeletedError;
            }
            else if (result == Helper.refernce_error_code)
            {
                response.message = Message.referenceError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.voucherCategoryDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(ReasonCategoryMaster_VM reasonCategoryMaster_VM)
        {
            if (reasonCategoryMaster_VM != null)
            {
                reasonCategoryMaster_VM.ClientId = clientId;
                if (reasonCategoryMaster_VM.Id > 0)
                {
                    if (_voucherReasonCategoryMasterRepository.Update(reasonCategoryMaster_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.voucherCategoryUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.voucherCategoryUpdatedError;
                    }
                }
                else
                {
                    if (_voucherReasonCategoryMasterRepository.Add(reasonCategoryMaster_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.voucherCategoryAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.voucherCategoryAddedError;
                    }
                }
            }
            return RedirectToAction("List", "VoucherReasonCategory");
        }
    }
}

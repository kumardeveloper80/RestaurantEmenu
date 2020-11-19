using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMenuApplication.Controllers
{
    public class VoucherReasonSubCategoryController : BaseController
    {
        private readonly IVoucherReasonSubCategoryMasterRepository _voucherReasonSubCategoryMasterRepository;
        private readonly IVoucherReasonCategoryMasterRepository _voucherReasonCategoryMasterRepository;
        public VoucherReasonSubCategoryController(
            IVoucherReasonSubCategoryMasterRepository voucherReasonSubCategoryMasterRepository,
            IVoucherReasonCategoryMasterRepository voucherReasonCategoryMasterRepository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _voucherReasonSubCategoryMasterRepository = voucherReasonSubCategoryMasterRepository;
            _voucherReasonCategoryMasterRepository = voucherReasonCategoryMasterRepository;
        }
        public IActionResult Index(int Id = 0)
        {
            ViewBag.ReasonCategories = new SelectList(_voucherReasonCategoryMasterRepository.GetList().Where(x => x.ClientId == clientId && x.Status), "Id", "Name");
            var subcategory = new ReasonSubCategoryMaster_VM();
            if (Id > 0)
            {
                subcategory = _voucherReasonSubCategoryMasterRepository.Get(Id);
                if (subcategory == null)
                {
                    return RedirectToAction("List", " VoucherReasonSubCategory");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(subcategory);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(subcategory);
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
            var result = _voucherReasonSubCategoryMasterRepository.GetList().Where(x => x.ClientId == clientId).ToList();

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
                    || x.ReasonCategoryName.ToLower().Contains(search.ToLower())
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
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ReasonCategoryName).ToList() : result.OrderBy(p => p.ReasonCategoryName).ToList();
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
            var result = _voucherReasonSubCategoryMasterRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.message = Message.voucherSubCategoryDeletedError;
            }
            else if (result == Helper.refernce_error_code)
            {
                response.message = Message.referenceError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.voucherSubCategoryDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(ReasonSubCategoryMaster_VM reasonSubCategoryMaster_VM)
        {
            if (reasonSubCategoryMaster_VM != null)
            {
                reasonSubCategoryMaster_VM.ClientId = clientId;
                if (reasonSubCategoryMaster_VM.Id > 0)
                {
                    if (_voucherReasonSubCategoryMasterRepository.Update(reasonSubCategoryMaster_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.voucherSubCategoryUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.voucherSubCategoryUpdatedError;
                    }
                }
                else
                {
                    if (_voucherReasonSubCategoryMasterRepository.Add(reasonSubCategoryMaster_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.voucherSubCategoryAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.voucherSubCategoryAddedError;
                    }
                }
            }
            return RedirectToAction("List", "VoucherReasonSubCategory");
        }
    }
}

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
    public class VoucherIssuanceController : BaseController
    {
        private readonly IVoucherIssuanceRepository _voucherIssuanceRepository;
        private readonly IVoucherReasonCategoryMasterRepository _voucherReasonCategoryMasterRepository;
        private readonly IVoucherReasonSubCategoryMasterRepository _voucherReasonSubCategoryMasterRepository;
        private readonly IVoucherSetupRepository _voucherSetupRepository;
        private readonly ICustomersRespository _customersRespository;
        List<ReasonCategoryMaster_VM> categories;
        List<ReasonSubCategoryMaster_VM> subCategories;
        public VoucherIssuanceController(IVoucherIssuanceRepository voucherIssuanceRepository,
            IVoucherReasonCategoryMasterRepository voucherReasonCategoryMasterRepository,
            IVoucherReasonSubCategoryMasterRepository voucherReasonSubCategoryMasterRepository,
            IVoucherSetupRepository voucherSetupRepository,
            ICustomersRespository customersRespository,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _voucherIssuanceRepository = voucherIssuanceRepository;
            _voucherReasonCategoryMasterRepository = voucherReasonCategoryMasterRepository;
            _voucherReasonSubCategoryMasterRepository = voucherReasonSubCategoryMasterRepository;
            _voucherSetupRepository = voucherSetupRepository;
            _customersRespository = customersRespository;
            categories = new List<ReasonCategoryMaster_VM>();
            subCategories = new List<ReasonSubCategoryMaster_VM>();
        }
        public IActionResult Index(int Id = 0)
        {
            if (hasVoucherIssuancePermission)
            {
                var voucherIssuance = new VoucherIssuance_VM();
                categories = _voucherReasonCategoryMasterRepository.GetList().Where(x => x.ClientId == clientId && x.Status).ToList();
                ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
                ViewBag.VoucherList = new SelectList(_voucherSetupRepository.GetVoucherForIssuance(loginUserId), "Id", "Name");
                ViewBag.CustomerList = new SelectList(_customersRespository.GetCustomersByClient(clientId), "Id", "FirstName");
                if (Id > 0)
                {
                    voucherIssuance = _voucherIssuanceRepository.Get(Id, this.loginUserId, isAdmin);
                    if (voucherIssuance == null)
                    {
                        return RedirectToAction("List", "VoucherIssuance");
                    }
                    else
                    {
                        subCategories = _voucherReasonSubCategoryMasterRepository.GetList().Where(x => x.ReasonCategoryId == voucherIssuance.ReasonCategoryId && x.ClientId == clientId).ToList();
                        ViewBag.SubCategoryList = new SelectList(subCategories, "Id", "Name");
                        ViewData["Title"] = "Edit";
                        return View(voucherIssuance);
                    }
                }
                else
                {
                    if (categories.Any())
                    {
                        subCategories = _voucherReasonSubCategoryMasterRepository.GetList().Where(x => x.ReasonCategoryId == categories[0].Id && x.ClientId == clientId).ToList();
                    }
                    ViewBag.SubCategoryList = new SelectList(subCategories, "Id", "Name");
                    ViewData["Title"] = "Add";
                    return View(voucherIssuance);
                }
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }

        public IActionResult List()
        {
            if (hasVoucherIssuancePermission || hasVoucherApprovalPermission)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Dashboard");
            }
        }

        [HttpPost]
        public IActionResult GetList(DataTableAjaxPostModel param)
        {
            int recordsTotal = 0;
            var result = _voucherIssuanceRepository.GetList(this.loginUserId, isAdmin);

            if (result.Any())
            {
                result.ForEach(x => x.HasVoucherApprovalPermission = hasVoucherApprovalPermission);
                result.ForEach(x => x.HasVoucherIssuancePermission = hasVoucherIssuancePermission);
                recordsTotal = result.Count;

                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.VoucherName.ToLower().Contains(search.ToLower())
                    || x.CustomerName.ToLower().Contains(search.ToLower())
                    || x.CategoryName.ToLower().Contains(search.ToLower())
                    || x.SubCategoryName.ToLower().Contains(search.ToLower())
                    || x.ApprovedStatus.ToLower().Contains(search.ToLower())
                    ).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.VoucherName).ToList() : result.OrderBy(p => p.VoucherName).ToList();
                    break;
                case "1":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.CustomerName).ToList() : result.OrderBy(p => p.CustomerName).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.CategoryName).ToList() : result.OrderBy(p => p.CategoryName).ToList();
                    break;
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.SubCategoryName).ToList() : result.OrderBy(p => p.SubCategoryName).ToList();
                    break;
                case "4":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ApprovedStatus).ToList() : result.OrderBy(p => p.ApprovedStatus).ToList();
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
            var result = _voucherIssuanceRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.voucherIssuanceDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.voucherIssuanceDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(VoucherIssuance_VM voucherIssuance_VM)
        {
            if (voucherIssuance_VM != null)
            {
                if (voucherIssuance_VM.Id > 0)
                {
                    if (_voucherIssuanceRepository.Update(voucherIssuance_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.voucherIssuanceUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.voucherIssuanceUpdatedError;
                    }
                }
                else
                {
                    if (_voucherIssuanceRepository.Add(voucherIssuance_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.voucherIssuanceAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.voucherIssuanceAddedError;
                    }
                }
            }
            return RedirectToAction("List", "VoucherIssuance");
        }

        [HttpGet]
        public IActionResult GetSubCategoryByCategoryId(int categoryId)
        {
            var menuItemList = new List<MenuItem_VM>();
            if (categoryId > 0)
            {
                subCategories = _voucherReasonSubCategoryMasterRepository.GetList().Where(x => x.ReasonCategoryId == categoryId && x.ClientId == clientId).ToList();
            }
            var list = new SelectList(subCategories, "Id", "Name");
            return Ok(list);
        }

        [HttpPost]
        public IActionResult Approve([FromBody] VoucherIssuance_VM voucherIssuance_VM)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            if (voucherIssuance_VM != null)
            {
                var result = _voucherIssuanceRepository.SetApproveStatus(voucherIssuance_VM, loginUserId);
                if (result == 0)
                {
                    response.status = Helper.failure_code;
                    response.message = "Voucher status change un-successfully";
                }
                else
                {
                    response.status = Helper.success_code;
                    response.message = voucherIssuance_VM.IsApproved.Value ? "Voucher approved successfully" : "Voucher rejected successfully";
                }
            }
            else
            {
                response.message = Message.badRequest;
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetVoucherStatus(int id)
        {
            var result = _voucherIssuanceRepository.GetVoucherApproveStatus(id);
            return PartialView("~/Views/VoucherIssuance/_VoucherApprovalPartialView.cshtml", result);
        }
    }
}
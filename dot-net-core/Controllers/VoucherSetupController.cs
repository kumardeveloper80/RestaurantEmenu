using System;
using System.Collections.Generic;
using System.Linq;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMenuApplication.Controllers
{
    public class VoucherSetupController : BaseController
    {
        private readonly IVoucherSetupRepository _voucherSetupRepository;
        private readonly IStoresRespository _storesRespository;
        private readonly IMenuItemRepository _menuItemRepository;
        public VoucherSetupController(IVoucherSetupRepository voucherSetupRepository, IStoresRespository storesRespository, IMenuItemRepository menuItemRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _voucherSetupRepository = voucherSetupRepository;
            _storesRespository = storesRespository;
            _menuItemRepository = menuItemRepository;
        }
        public IActionResult Index(int Id = 0)
        {
            ViewBag.StoresList = new SelectList(_storesRespository.GetStoresByUserId(this.loginUserId), "Id", "StoreName");
            ViewBag.SurveyList = new List<SurveyForm_VM>();
            ViewBag.MenuItemList = new List<MenuItem_VM>();
            var voucherSetup = new VoucherSetup_VM();
            if (Id > 0)
            {
                voucherSetup = _voucherSetupRepository.Get(Id, this.loginUserId, isAdmin);
                if (voucherSetup == null)
                {
                    return RedirectToAction("List", "VoucherSetup");
                }
                else
                {
                    ViewBag.MenuItemList = new SelectList(_menuItemRepository.GetMenuItemForVoucherSetUp(this.loginUserId, clientId).Where(c => voucherSetup.StoreIds.Contains(c.StoreId)).ToList(), "Id", "Name");
                    ViewData["Title"] = "Edit";
                    return View(voucherSetup);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(voucherSetup);
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
            var result = _voucherSetupRepository.GetList(this.loginUserId, isAdmin);

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
                    || x.Value.ToString().Contains(search.ToLower())
                    || x.Usage.ToLower().Contains(search.ToLower())
                    || x.StartDate.ToLower().Contains(search.ToLower())
                    || x.EndDate.ToLower().Contains(search.ToLower())
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
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Value).ToList() : result.OrderBy(p => p.Value).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Usage).ToList() : result.OrderBy(p => p.Usage).ToList();
                    break;
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.StartDate).ToList() : result.OrderBy(p => p.StartDate).ToList();
                    break;
                case "4":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.EndDate).ToList() : result.OrderBy(p => p.EndDate).ToList();
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
            var result = _voucherSetupRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.voucherSetupDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.voucherSetupDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(VoucherSetup_VM voucherSetup_VM)
        {
            if (voucherSetup_VM != null)
            {
                if (voucherSetup_VM.Id > 0)
                {
                    if (_voucherSetupRepository.Update(voucherSetup_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.voucherSetupUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.voucherSetupUpdatedError;
                    }
                }
                else
                {
                    if (_voucherSetupRepository.Add(voucherSetup_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.voucherSetupAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.voucherSetupAddedError;
                    }
                }
            }
            return RedirectToAction("List", "VoucherSetup");
        }

        [HttpGet]
        public IActionResult GetMenuItemListByStoreId(string storeIds)
        {
            var menuItemList = new List<MenuItem_VM>();
            if (storeIds != null)
            {
                var stores = Helper.StringToIntList(storeIds);
                menuItemList = _menuItemRepository.GetMenuItemForVoucherSetUp(this.loginUserId, clientId).Where(c => stores.Contains(c.StoreId)).ToList();
            }
            var menuItems = new SelectList(menuItemList, "Id", "Name");
            return Ok(menuItems);
        }
    }
}

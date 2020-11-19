using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace EMenuApplication.Controllers
{
    public class MenuScheduleController : BaseController
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IMenuScheduleRepository _menuScheduleRepository;
        private readonly IStoresRespository _storesRespository;

        public List<Menu_VM> menuList { get; set; }
        public MenuScheduleController(IMenuRepository menuRepository, IMenuScheduleRepository menuScheduleRepository, IStoresRespository storesRespository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _menuRepository = menuRepository;
            _menuScheduleRepository = menuScheduleRepository;
            _storesRespository = storesRespository;
            menuList = new List<Menu_VM>();
        }

        public IActionResult Index(int Id = 0)
        {
            var menuSchedule = new MenuSchedule_VM();
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName");
            ViewBag.StoresList = new SelectList(_storesRespository.GetStoresByUserId(this.loginUserId), "Id", "StoreName");
            if (Id > 0)
            {
                menuSchedule = _menuScheduleRepository.Get(Id, this.loginUserId, isAdmin);
                if (menuSchedule == null)
                {
                    return RedirectToAction("List", "MenuSchedule");
                }
                else
                {
                    menuSchedule.QRCode = Helper.GenerateQRCode(QRCodeUrl + Helper.Convert_StringvalueToHexvalue(menuSchedule.UniqueCode, System.Text.Encoding.Unicode));
                    menuList = _menuRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.ConceptId == menuSchedule.ConceptId).ToList();
                    ViewBag.Menus = new SelectList((from x in menuList
                                                    select new
                                                    {
                                                        Id = x.Id,
                                                        MenuCode = x.Name + "(" + x.Code + ")"
                                                    }).ToList(), "Id", "MenuCode");
                    ViewData["Title"] = "Edit";
                    return View(menuSchedule);
                }
            }
            else
            {
                if (conceptsList.Any())
                {
                    if (SelectedConceptId > 0)
                    {
                        menuList = _menuRepository.GetList(this.loginUserId, isAdmin).Where(x => x.ConceptId == SelectedConceptId).ToList();
                        menuSchedule.ConceptId = SelectedConceptId;
                    }
                    else
                    {
                        menuList = _menuRepository.GetList(this.loginUserId, isAdmin).Where(x => x.ConceptId == conceptsList[0].Id).ToList();
                    }
                }

                ViewBag.Menus = new SelectList((from x in menuList
                                                select new
                                                {
                                                    Id = x.Id,
                                                    MenuCode = x.Name + "(" + x.Code + ")"
                                                }).ToList(), "Id", "MenuCode");

                ViewData["Title"] = "Add";
                return View(menuSchedule);
            }
        }

        public IActionResult List()
        {
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName", conceptsList.Where(x => x.Id == SelectedConceptId).Select(x => x.Id).FirstOrDefault());
            ViewBag.StoresList = new SelectList(_storesRespository.GetStoresByUserId(this.loginUserId), "Id", "StoreName");

            ViewBag.MenuList = new SelectList((from x in _menuRepository.GetList(this.loginUserId, isAdmin).Where(x => x.ConceptId == SelectedConceptId && x.Status)
                                               select new
                                               {
                                                   Id = x.Id,
                                                   MenuCode = x.Name + "(" + x.Code + ")"
                                               }).ToList(), "Id", "MenuCode");
            return View();
        }

        [HttpPost]
        public IActionResult GetList(DataTableAjaxPostModel param)
        {
            int recordsTotal = 0;
            var result = _menuScheduleRepository.GetList(this.loginUserId, this._configuration.GetValue<string>("QRCodeURL"), isAdmin);

            if (result.Any())
            {
                recordsTotal = result.Count;

                if (param.conceptId > 0)
                {
                    result = result.Where(x => x.ConceptId == param.conceptId).ToList();
                }

                if (param.storeId > 0)
                {
                    result = result.Where(x => x.StoreId == param.storeId).ToList();
                }

                if (param.menuId > 0)
                {
                    result = result.Where(x => x.MenuId == param.menuId).ToList();
                }


                if (param.status == Helper.Active)
                {
                    result = result.Where(x => x.Status == true).ToList();
                }
                else if (param.status == Helper.Inactive)
                {
                    result = result.Where(x => !x.Status).ToList();
                }
                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.Code.ToLower().Contains(search.ToLower())
                    || x.MenuName.ToLower().Contains(search.ToLower())
                    || x.ConceptName.ToLower().Contains(search.ToLower())
                    || x.StoreName.ToLower().Contains(search.ToLower())
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
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Code).ToList() : result.OrderBy(p => p.Code).ToList();
                    break;
                case "1":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.MenuName).ToList() : result.OrderBy(p => p.MenuName).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ConceptName).ToList() : result.OrderBy(p => p.ConceptName).ToList();
                    break;
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.StoreName).ToList() : result.OrderBy(p => p.StoreName).ToList();
                    break;
                case "4":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.StartDate).ToList() : result.OrderBy(p => p.StartDate).ToList();
                    break;
                case "5":
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
            var result = _menuScheduleRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.menuScheduleDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.menuScheduleDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveAsync(MenuSchedule_VM menuSchedule_VM)
        {
            if (menuSchedule_VM != null)
            {
                if (menuSchedule_VM.Id > 0)
                {
                    var result = _menuScheduleRepository.Update(menuSchedule_VM, this.loginUserId);
                    if (result > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.menuScheduleUpdated;
                    }
                    else if (result == Helper.onsamedate_code)
                    {
                        TempData["Message"] = Message.menuScheduleOnSameDateError;
                    }
                    else
                    {
                        TempData["Message"] = Message.menuScheduleUpdatedError;
                    }
                }
                else
                {
                    var ret = _menuScheduleRepository.Add(menuSchedule_VM, this.loginUserId);
                    if (ret > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.menuScheduleAdded;
                    }
                    else if (ret == Helper.onsamedate_code)
                    {
                        TempData["Message"] = Message.menuScheduleOnSameDateError;
                    }
                    else
                    {
                        TempData["Message"] = Message.menuScheduleAddedError;
                    }
                }
            }
            return RedirectToAction("List", "MenuSchedule");
        }

        [HttpGet]
        public IActionResult CheckUniqueCode(int id, string code, int conceptId)
        {
            return Ok(_menuScheduleRepository.IsUniqueMenuScheduleCode(id, code, conceptId, this.loginUserId, isAdmin));
        }

        [HttpGet]
        public IActionResult GetMenuList(int conceptId)
        {
            var menus = _menuRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.ConceptId == conceptId && x.Status).ToList();
            var result = new SelectList((from x in menus
                                         select new
                                         {
                                             Id = x.Id,
                                             MenuCode = x.Name + "(" + x.Code + ")"
                                         }).ToList(), "Id", "MenuCode");
            return Ok(result);
        }
    }
}

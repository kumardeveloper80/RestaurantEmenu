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
using MoreLinq;

namespace EMenuApplication.Controllers
{
    public class MenuController : BaseController
    {
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly IMenuRepository _menuRepository;

        public List<MenuItem_VM> menuItemList { get; set; }
        public List<Category_VM> categoryList { get; set; }
        public List<Menu_VM> menuList { get; set; }
        public MenuController(IMenuItemRepository menuItemRepository, IMenuRepository menuRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _menuItemRepository = menuItemRepository;
            _menuRepository = menuRepository;
            menuItemList = new List<MenuItem_VM>();
            categoryList = new List<Category_VM>();
            menuList = new List<Menu_VM>();
        }
        public IActionResult Index(int Id = 0)
        {
            var menu = new Menu_VM();
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName");
            if (Id > 0)
            {
                menu = _menuRepository.Get(Id, this.loginUserId, isAdmin);
                if (menu == null)
                {
                    return RedirectToAction("List", "Menu");
                }
                else
                {
                    menuItemList = _menuItemRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.MenuItemConcepts.Any(x => x.ConceptId == menu.ConceptId)).ToList();
                    if (menuItemList.Any())
                    {
                        categoryList = menuItemList.Select(x => new Category_VM()
                        {
                            Id = x.CategoryId,
                            Name = x.CategoryName
                        }).DistinctBy(x => x.Name).ToList();

                        categoryList.Add(new Category_VM { Id = 0, Name = "All" });
                    }


                    ViewBag.MenuItems = new SelectList(menuItemList, "Id", "Name");
                    ViewBag.Categories = new SelectList(categoryList.OrderBy(x => x.Id), "Id", "Name");
                    ViewData["Title"] = "Edit";
                    return View(menu);
                }
            }
            else
            {
                if (conceptsList.Any())
                {
                    if (SelectedConceptId > 0)
                    {
                        menuItemList = _menuItemRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.MenuItemConcepts.Any(x => x.ConceptId == SelectedConceptId)).ToList();
                        menu.ConceptId = SelectedConceptId;
                    }
                    else
                    {
                        menuItemList = _menuItemRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.MenuItemConcepts.Any(x => x.ConceptId == conceptsList[0].Id)).ToList();
                    }

                    if (menuItemList.Any())
                    {
                        categoryList = menuItemList.Select(x => new Category_VM()
                        {
                            Id = x.CategoryId,
                            Name = x.CategoryName
                        }).DistinctBy(x => x.Name).ToList();
                        categoryList.Add(new Category_VM { Id = 0, Name = "All" });
                    }
                }

                ViewBag.MenuItems = new SelectList(menuItemList, "Id", "Name");
                ViewBag.Categories = new SelectList(categoryList.OrderBy(x => x.Id), "Id", "Name");
                ViewData["Title"] = "Add";
                return View(menu);
            }
        }

        public IActionResult List()
        {
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName", conceptsList.Where(x => x.Id == SelectedConceptId).Select(x => x.Id).FirstOrDefault());
            ViewBag.MenuList = new SelectList(_menuRepository.GetList(loginUserId, isAdmin).Where(x => x.ConceptId == SelectedConceptId && x.Status).ToList(), "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult GetList(DataTableAjaxPostModel param)
        {
            int recordsTotal = 0;
            var result = _menuRepository.GetList(this.loginUserId, isAdmin);

            if (result.Any())
            {
                recordsTotal = result.Count;

                if (param.conceptId > 0)
                {
                    result = result.Where(x => x.ConceptId == param.conceptId).ToList();
                }

                if (param.menuIds.Any())
                {
                    result = result.Where(x => param.menuIds.Contains(x.Id)).ToList();
                }

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
                    result = result.Where(x => x.Code.ToLower().Contains(search.ToLower())
                   || x.Name.ToLower().Contains(search.ToLower())
                   || x.ConceptName.ToLower().Contains(search.ToLower())
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
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Name).ToList() : result.OrderBy(p => p.Name).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ConceptName).ToList() : result.OrderBy(p => p.ConceptName).ToList();
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
            var result = _menuRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.menuDeletedError;
            }
            else if (result == Helper.refernce_error_code)
            {
                response.message = Message.referenceError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.menuDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveAsync(Menu_VM menu_VM)
        {
            if (menu_VM != null)
            {
                if (menu_VM.Id > 0)
                {
                    if (_menuRepository.Update(menu_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.menuUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.menuItemUpdatedError;
                    }
                }
                else
                {
                    if (_menuRepository.Add(menu_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.menuAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.menuAddedError;
                    }
                }
            }
            return RedirectToAction("List", "Menu");
        }

        [HttpGet]
        public IActionResult CheckUniqueCode(int id, string code, int conceptId)
        {
            return Ok(_menuRepository.IsUniqueMenuCode(id, code, conceptId, this.loginUserId, isAdmin));
        }

        [HttpGet]
        public IActionResult CheckUniqueName(int id, string name, int conceptId)
        {
            return Ok(_menuRepository.IsUniqueMenuName(id, name, conceptId, this.loginUserId, isAdmin));
        }

        [HttpGet]
        public IActionResult GetMenuItemList(int conceptId)
        {
            menuItemList = _menuItemRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.MenuItemConcepts.Any(x => x.ConceptId == conceptId)).ToList();
            var menuItems = new SelectList(menuItemList, "Id", "Name");
            var categories = new SelectList(menuItemList.Select(x => new Category_VM()
            {
                Id = x.CategoryId,
                Name = x.CategoryName
            }).DistinctBy(x => x.Name).ToList(), "Id", "Name");
            var result = new { categories = categories, menuItems = menuItems };
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetMenuItemByCateogry(int conceptId, int categoryId)
        {
            menuItemList = _menuItemRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.MenuItemConcepts.Any(x => x.ConceptId == conceptId)).ToList();
            if (categoryId > 0)
            {
                menuItemList = menuItemList.Where(x => x.CategoryId == categoryId).ToList();
            }
            var menuItems = new SelectList(menuItemList, "Id", "Name");
            return Ok(menuItems);
        }

        public IActionResult CategorySequence()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetMenuCategory(DataTableAjaxPostModel param)
        {
            var result = _menuRepository.GetMenuCategory(param.menuId, this.loginUserId, isAdmin);
            return Json(new
            {
                draw = param.draw,
                data = result
            });
        }

        [HttpPost]
        public IActionResult ManageCategorySequence([FromBody] List<CategorySequence_VM> categorySequence_VMs)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            if (categorySequence_VMs != null && categorySequence_VMs.Any())
            {
                var result = _menuRepository.ManageCategorySequence(categorySequence_VMs);
                if (result == 0)
                {
                    response.status = Helper.failure_code;
                    response.message = Message.categorySequenceError;
                }
                else
                {
                    response.status = Helper.success_code;
                    response.message = Message.categorySequence;
                }
            }
            return Ok(response);
        }

        public IActionResult ItemSequence()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GetMenuItem(DataTableAjaxPostModel param)
        {
            var result = _menuRepository.GetMenuItem(param.menuId, param.categoryId, this.loginUserId, isAdmin);
            return Json(new
            {
                draw = param.draw,
                data = result
            });
        }

        [HttpPost]
        public IActionResult ManageMenuItemSequence([FromBody] List<ItemSequence_VM> itemSequence_VMs)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            if (itemSequence_VMs != null && itemSequence_VMs.Any())
            {
                var result = _menuRepository.ManageMenuItemSequence(itemSequence_VMs);
                if (result == 0)
                {
                    response.status = Helper.failure_code;
                    response.message = Message.mneuItemSequenceError;
                }
                else
                {
                    response.status = Helper.success_code;
                    response.message = Message.mneuItemSequence;
                }
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult GetMenu(int conceptId)
        {
            if (conceptId > 0)
            {
                menuList = _menuRepository.GetList(loginUserId, isAdmin).Where(x => x.ConceptId == conceptId && x.Status).ToList();
            }
            else
            {
                menuList = new List<Menu_VM>();
            }
            var result = new SelectList(menuList, "Id", "Name");
            return Ok(result);
        }
    }
}

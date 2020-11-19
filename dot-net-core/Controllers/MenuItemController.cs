using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EMenuApplication.ViewModels;
using EMenuApplication.Repository.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using EMenuApplication.Utility;
using System;
using System.Collections.Generic;

namespace EMenuApplication.Controllers
{
    public class MenuItemController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IItemTagRepository _itemTagRepository;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ICurrencyRepository _currencyRepository;
        public List<Category_VM> categoryList { get; set; }
        public List<MenuItem_VM> menuItemList { get; set; }
        public List<ItemTag_VM> itemTagList { get; set; }
        public MenuItemController(ICategoryRepository categoryRepository, IItemTagRepository itemTagRepository, IMenuItemRepository menuItemRepository, ICurrencyRepository
            currencyRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _categoryRepository = categoryRepository;
            _itemTagRepository = itemTagRepository;
            _menuItemRepository = menuItemRepository;
            _currencyRepository = currencyRepository;

            categoryList = new List<Category_VM>();
            itemTagList = new List<ItemTag_VM>();
            menuItemList = new List<MenuItem_VM>();
        }
        public IActionResult Index(int Id = 0)
        {
            var menuItem = new MenuItem_VM();
            ViewBag.Currency = new SelectList(_currencyRepository.GetByClientId(clientId), "Id", "Currency");
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName");
            if (Id > 0)
            {
                menuItem = _menuItemRepository.Get(Id, this.loginUserId, isAdmin);
                if (menuItem == null)
                {
                    return RedirectToAction("List", "MenuItem");
                }
                else
                {
                    categoryList = _categoryRepository.GetList(this.loginUserId, isAdmin).Where(p => p.CategoryConcepts.Any(c => menuItem.ConceptIds.Any(c2 => c2 == c.Id))).ToList();
                    itemTagList = _itemTagRepository.GetList(this.loginUserId, isAdmin).Where(p => p.ItemTagsConcepts.Any(c => menuItem.ConceptIds.Any(c2 => c2 == c.Id))).ToList();
                    ViewBag.Categories = new SelectList(categoryList, "Id", "Name");
                    ViewBag.ItemTags = new SelectList(itemTagList, "Id", "Name");
                    return View(menuItem);
                }
            }
            else
            {
                if (conceptsList.Any())
                {
                    if (SelectedConceptId > 0)
                    {
                        menuItem.ConceptIds = new List<int>() { conceptsList.Where(x => x.Id == SelectedConceptId).Select(x => x.Id).FirstOrDefault() };
                        categoryList = _categoryRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.CategoryConcepts.Any(x => x.Id == SelectedConceptId)).ToList();
                        itemTagList = _itemTagRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.ItemTagsConcepts.Any(x => x.Id == SelectedConceptId)).ToList();
                    }
                    else
                    {
                        menuItem.ConceptIds = new List<int>() { conceptsList[0].Id };
                        categoryList = _categoryRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.CategoryConcepts.Any(x => x.Id == conceptsList[0].Id)).ToList();
                        itemTagList = _itemTagRepository.GetList(this.loginUserId, isAdmin).Where(x => x.Status && x.ItemTagsConcepts.Any(x => x.Id == conceptsList[0].Id)).ToList();
                    }
                }

                ViewBag.Categories = new SelectList(categoryList, "Id", "Name");
                ViewBag.ItemTags = new SelectList(itemTagList, "Id", "Name");

                ViewData["Title"] = "Add";
                return View(menuItem);
            }
        }

        public IActionResult List()
        {
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName", conceptsList.Where(x => x.Id == SelectedConceptId).Select(x => x.Id).FirstOrDefault());
            ViewBag.CategoryList = new SelectList(_categoryRepository.GetList(loginUserId, isAdmin).Where(x => x.CategoryConcepts.Any(y => y.Id == SelectedConceptId) && x.Status).ToList(), "Id", "Name");
            //ViewBag.MenuItemList = new SelectList(_menuItemRepository.GetList(loginUserId, isAdmin).Where(x => x.MenuItemConcepts.Any(y => y.ConceptId == SelectedConceptId) && x.Status).ToList(), "Id", "Name");
            ViewBag.MenuItemList = new SelectList(menuItemList, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult GetList(DataTableAjaxPostModel param)
        {
            int recordsTotal = 0;
            var result = _menuItemRepository.GetList(this.loginUserId, isAdmin);

            if (result.Any())
            {
                recordsTotal = result.Count;

                if (param.conceptId > 0)
                {
                    result = result.Where(x => x.MenuItemConcepts.Any(x => x.ConceptId == param.conceptId)).ToList();
                }

                if (param.categoryId > 0)
                {
                    result = result.Where(x => x.CategoryId == param.categoryId).ToList();
                }

                if (param.menuItemId > 0)
                {
                    result = result.Where(x => x.Id == param.menuItemId).ToList();
                }


                if (param.status == Helper.Active)
                {
                    result = result.Where(x => x.Status).ToList();
                }
                else if (param.status == Helper.Inactive)
                {
                    result = result.Where(x => !x.Status).ToList();
                }

                result.ForEach(x => x.ConceptName = string.Join(", ", x.MenuItemConcepts.Select(x => x.ConceptName).ToList()));

                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.PLU.ToLower().Contains(search.ToLower())
                        || x.Name.ToLower().Contains(search.ToLower())
                        || x.ConceptName.ToLower().Contains(search.ToLower())
                        || x.CategoryName.ToLower().Contains(search.ToLower())
                        || x.Currency.ToLower().Contains(search.ToLower())
                        || x.Price.ToString().Contains(search.ToLower())
                        || x.LabelEN.ToLower().Contains(search.ToLower())
                        || x.LabelAR.ToLower().Contains(search.ToLower())
                        ).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.PLU).ToList() : result.OrderBy(p => p.PLU).ToList();
                    break;
                case "1":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Name).ToList() : result.OrderBy(p => p.Name).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ConceptName).ToList() : result.OrderBy(p => p.ConceptName).ToList();
                    break;
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.CategoryName).ToList() : result.OrderBy(p => p.CategoryName).ToList();
                    break;
                case "4":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Currency).ToList() : result.OrderBy(p => p.Currency).ToList();
                    break;
                case "5":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Price).ToList() : result.OrderBy(p => p.Price).ToList();
                    break;
                case "6":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.LabelEN).ToList() : result.OrderBy(p => p.LabelEN).ToList();
                    break;
                case "7":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.LabelAR).ToList() : result.OrderBy(p => p.LabelAR).ToList();
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
            var result = _menuItemRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.menuItemDeletedError;
            }
            else if (result == Helper.refernce_error_code)
            {
                response.message = Message.referenceError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.menuItemDeleted;
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult CheckUniquePLU(int id, string plu, string conceptIds)
        {
            if (conceptIds != null)
            {
                return Ok(_menuItemRepository.IsUniqueMenuItemPLU(id, plu, Helper.StringToIntList(conceptIds), this.loginUserId, isAdmin));
            }
            return Ok(true);
        }

        [HttpGet]
        public IActionResult CheckUniqueName(int id, string name, string conceptIds)
        {
            if (conceptIds != null)
            {
                return Ok(_menuItemRepository.IsUniqueMenuItemName(id, name, Helper.StringToIntList(conceptIds), this.loginUserId, isAdmin));
            }
            return Ok(true);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAsync(MenuItem_VM menuItem_VM)
        {
            if (menuItem_VM != null)
            {
                if (menuItem_VM.ThumbnailImage != null && menuItem_VM.ThumbnailImage.Length > 0)
                {
                    menuItem_VM.ThumbnailImageName = await Helper.FileUploadAsync(path, menuItem_VM.ThumbnailImage);
                }

                if (menuItem_VM.LargeImage != null && menuItem_VM.LargeImage.Length > 0)
                {
                    menuItem_VM.LargeImageName = await Helper.FileUploadAsync(path, menuItem_VM.LargeImage);
                }

                if (menuItem_VM.OverLayImage != null && menuItem_VM.OverLayImage.Length > 0)
                {
                    menuItem_VM.OverLayImageName = await Helper.FileUploadAsync(path, menuItem_VM.OverLayImage);
                }

                if (menuItem_VM.Id > 0)
                {
                    if (_menuItemRepository.Update(menuItem_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.menuItemUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.menuItemUpdatedError;
                    }
                }
                else
                {
                    if (_menuItemRepository.Add(menuItem_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.menuItemAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.menuItemAddedError;
                    }
                }
            }
            return RedirectToAction("List", "MenuItem");
        }

        [HttpGet]
        public IActionResult GetCategoryAndItemTagList(string conceptIds)
        {
            if (conceptIds != null)
            {
                var concepts = Helper.StringToIntList(conceptIds);
                categoryList = _categoryRepository.GetList(this.loginUserId, isAdmin).Where(p => p.CategoryConcepts.Any(c => concepts.Any(c2 => c2 == c.Id))).ToList();
                itemTagList = _itemTagRepository.GetList(this.loginUserId, isAdmin).Where(p => p.ItemTagsConcepts.Any(c => concepts.Any(c2 => c2 == c.Id))).ToList();
            }
            else
            {
                categoryList = new List<Category_VM>();
                itemTagList = new List<ItemTag_VM>();
            }
            var categories = new SelectList(categoryList, "Id", "Name");
            var itemTags = new SelectList(itemTagList, "Id", "Name");

            var result = new { categories = categories, itemTags = itemTags };
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetCategory(int conceptId)
        {
            if (conceptId > 0)
            {
                categoryList = _categoryRepository.GetList(this.loginUserId, isAdmin).Where(x => x.CategoryConcepts.Any(y => y.Id == conceptId) && x.Status).ToList();
                //menuItemList = _menuItemRepository.GetList(this.loginUserId, isAdmin).Where(x => x.MenuItemConcepts.Any(y => y.ConceptId == conceptId) && x.Status).ToList();
            }
            else
            {
                categoryList = new List<Category_VM>();
                //menuItemList = new List<MenuItem_VM>();
            }
            var categories = new SelectList(categoryList, "Id", "Name");
            var menuItems = new SelectList(menuItemList, "Id", "Name");
            var result = new { categories = categories, menuItems = menuItems };
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetMenuItems(int categoryId)
        {
            if (categoryId > 0)
            {
                menuItemList = _menuItemRepository.GetList(this.loginUserId, isAdmin).Where(x => x.CategoryId == categoryId && x.Status).ToList();
            }
            else
            {
                menuItemList = new List<MenuItem_VM>();
            }
            var menuItems = new SelectList(menuItemList, "Id", "Name");
            var result = new { menuItems = menuItems };
            return Ok(result);
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace EMenuApplication.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryRepository _categoryRepository;

        public List<Category_VM> categoryList { get; set; }
        public CategoryController(ICategoryRepository categoryRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _categoryRepository = categoryRepository;
            categoryList = new List<Category_VM>();
        }

        public IActionResult Index(int Id = 0)
        {
            var category = new Category_VM();
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName");
            if (Id > 0)
            {
                category = _categoryRepository.Get(Id, this.loginUserId,isAdmin);
                if (category == null)
                {
                    return RedirectToAction("List", "Category");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(category);
                }
            }
            else
            {
                if (conceptsList.Any())
                {
                    if (SelectedConceptId > 0)
                    {
                        category.ConceptIds = new List<int>() { conceptsList.Where(x => x.Id == SelectedConceptId).Select(x => x.Id).FirstOrDefault() };
                    }
                    else
                    {
                        category.ConceptIds = new List<int>() { conceptsList[0].Id };
                    }
                }
                ViewData["Title"] = "Add";
                return View(category);
            }
        }

        public IActionResult List()
        {
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName", conceptsList.Where(x => x.Id == SelectedConceptId).Select(x => x.Id).FirstOrDefault());
            ViewBag.CategoryList = _categoryRepository.GetList(loginUserId, isAdmin).Where(x => x.CategoryConcepts.Any(x => x.Id == SelectedConceptId) && x.Status).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult GetList(DataTableAjaxPostModel param)
        {
            int recordsTotal = 0;
            var result = _categoryRepository.GetList(this.loginUserId, isAdmin);

            if (result.Any())
            {
                recordsTotal = result.Count;

                if (param.conceptId > 0)
                {
                    result = result.Where(x => x.CategoryConcepts.Any(x => x.Id == param.conceptId)).ToList();
                }

                if (param.nameIds.Any())
                {
                    result = result.Where(x => param.nameIds.Contains(x.Id)).ToList();
                }

                if (param.labelENIds.Any())
                {
                    result = result.Where(x => param.labelENIds.Contains(x.Id)).ToList();
                }

                if (param.labelARIds.Any())
                {
                    result = result.Where(x => param.labelARIds.Contains(x.Id)).ToList();
                }

                if (param.status == Helper.Active)
                {
                    result = result.Where(x => x.Status).ToList();
                }
                else if (param.status == Helper.Inactive)
                {
                    result = result.Where(x => !x.Status).ToList();
                }

                result.ForEach(x => x.ConceptName = string.Join(", ", x.CategoryConcepts.Select(x => x.ConceptName).ToList()));

                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.Code.ToLower().Contains(search.ToLower())
                        || x.Name.ToLower().Contains(search.ToLower())
                        || x.ConceptName.ToLower().Contains(search.ToLower())
                        || x.LabelAR.ToLower().Contains(search.ToLower())
                        || x.LabelEN.ToLower().Contains(search.ToLower())).ToList();
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
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.LabelEN).ToList() : result.OrderBy(p => p.LabelEN).ToList();
                    break;
                case "4":
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
            var result = _categoryRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.categoryDeletedError;
            }
            else if (result == Helper.refernce_error_code)
            {
                response.message = Message.referenceError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.categoryDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAsync(Category_VM category_VM)
        {
            if (category_VM != null)
            {
                if (category_VM.Image != null && category_VM.Image.Length > 0)
                {
                    category_VM.ImageName = await Helper.FileUploadAsync(path, category_VM.Image);
                }
                if (category_VM.Id > 0)
                {
                    if (_categoryRepository.Update(category_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.categoryUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.categoryUpdateError;
                    }
                }
                else
                {
                    if (_categoryRepository.Add(category_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.categoryAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.categoryAddedError;
                    }
                }
            }
            return RedirectToAction("List", "Category");
        }

        [HttpGet]
        public IActionResult CheckUniqueName(int id, string name, string conceptIds)
        {
            if (conceptIds != null)
            {
                return Ok(_categoryRepository.IsUniqueCategoryName(id, name, Helper.StringToIntList(conceptIds), this.loginUserId, isAdmin));
            }
            return Ok(true);
        }

        [HttpGet]
        public IActionResult CheckUniqueCode(int id, string code, string conceptIds)
        {
            if (conceptIds != null)
            {
                return Ok(_categoryRepository.IsUniqueCategoryCode(id, code, Helper.StringToIntList(conceptIds), this.loginUserId, isAdmin));
            }
            return Ok(true);
        }

        [HttpGet]
        public IActionResult GetCategory(int conceptId)
        {
            if (conceptId > 0)
            {
                categoryList = _categoryRepository.GetList(loginUserId, isAdmin).Where(x => x.CategoryConcepts.Any(x => x.Id == conceptId) && x.Status).ToList();
            }
            else
            {
                categoryList = new List<Category_VM>();
            }
            return Ok(categoryList);
        }
    }
}

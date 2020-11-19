using System;
using System.Linq;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace EMenuApplication.Controllers
{
    public class ItemTagController : BaseController
    {
        private readonly IItemTagRepository _itemTagRepository;

        public List<ItemTag_VM> itemTagList { get; set; }
        public ItemTagController(IItemTagRepository itemTagRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _itemTagRepository = itemTagRepository;
            itemTagList = new List<ItemTag_VM>();
        }

        public IActionResult Index(int Id = 0)
        {
            var itemTag = new ItemTag_VM();
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName");
            if (Id > 0)
            {
                itemTag = _itemTagRepository.Get(Id, this.loginUserId, isAdmin);
                if (itemTag == null)
                {
                    return RedirectToAction("List", "ItemTag");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(itemTag);
                }
            }
            else
            {
                if (conceptsList.Any())
                {
                    if (SelectedConceptId > 0)
                    {
                        itemTag.ConceptIds = new List<int>() { conceptsList.Where(x => x.Id == SelectedConceptId).Select(x => x.Id).FirstOrDefault() };
                    }
                    else
                    {
                        itemTag.ConceptIds = new List<int>() { conceptsList[0].Id };
                    }

                }
                ViewData["Title"] = "Add";
                return View(itemTag);
            }
        }

        public IActionResult List()
        {
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName", conceptsList.Where(x => x.Id == SelectedConceptId).Select(x => x.Id).FirstOrDefault());
            ViewBag.ItemTagList = _itemTagRepository.GetList(loginUserId, isAdmin).Where(x => x.ItemTagsConcepts.Any(x => x.Id == SelectedConceptId) && x.Status).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult GetList(DataTableAjaxPostModel param)
        {
            int recordsTotal = 0;
            var result = _itemTagRepository.GetList(this.loginUserId, isAdmin);

            if (result.Any())
            {
                recordsTotal = result.Count;

                if (param.conceptId > 0)
                {
                    result = result.Where(x => x.ItemTagsConcepts.Any(x => x.Id == param.conceptId)).ToList();
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

                result.ForEach(x => x.ConceptName = string.Join(", ", x.ItemTagsConcepts.Select(x => x.ConceptName).ToList()));
                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.Name.ToLower().Contains(search.ToLower())
                    || x.LabelAR.ToLower().Contains(search.ToLower())
                    || x.ConceptName.ToLower().Contains(search.ToLower())
                    || x.LabelEN.ToLower().Contains(search.ToLower())
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
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ConceptName).ToList() : result.OrderBy(p => p.ConceptName).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.LabelEN).ToList() : result.OrderBy(p => p.LabelEN).ToList();
                    break;
                case "3":
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
            var result = _itemTagRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.message = Message.itemTagDeletedError;
            }
            else if (result == Helper.refernce_error_code)
            {
                response.message = Message.referenceError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.itemTagDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAsync(ItemTag_VM itemTag_VM)
        {
            if (itemTag_VM != null)
            {
                if (itemTag_VM.IconTag != null && itemTag_VM.IconTag.Length > 0)
                {
                    itemTag_VM.IconTagName = await Helper.FileUploadAsync(path, itemTag_VM.IconTag);
                }

                if (itemTag_VM.Id > 0)
                {
                    if (_itemTagRepository.Update(itemTag_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.itemTagUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.itemTagUpdatedError;
                    }
                }
                else
                {
                    if (_itemTagRepository.Add(itemTag_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.itemTagAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.itemTagAddedError;
                    }
                }
            }
            return RedirectToAction("List", "ItemTag");
        }

        [HttpGet]
        public IActionResult CheckUniqueName(int id, string name, string conceptIds)
        {
            if (conceptIds != null)
            {
                return Ok(_itemTagRepository.IsUniqueItemTagName(id, name, Helper.StringToIntList(conceptIds), this.loginUserId, isAdmin));
            }
            return Ok(true);
        }

        [HttpGet]
        public IActionResult GetItemTag(int conceptId)
        {
            if (conceptId > 0)
            {
                itemTagList = _itemTagRepository.GetList(loginUserId, isAdmin).Where(x => x.ItemTagsConcepts.Any(x => x.Id == conceptId) && x.Status).ToList();
            }
            else
            {
                itemTagList = new List<ItemTag_VM>();
            }
            return Ok(itemTagList);
        }
    }
}

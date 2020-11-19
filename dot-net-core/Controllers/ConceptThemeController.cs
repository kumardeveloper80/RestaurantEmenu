using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Controllers
{
    public class ConceptThemeController : BaseController
    {
        private readonly IConceptThemeRepository _conceptThemeRepository;
        public ConceptThemeController(IConceptThemeRepository conceptThemeRepository,  IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _conceptThemeRepository = conceptThemeRepository;
        }
        
        public IActionResult Index(int Id = 0)
        {
            ViewBag.ConceptsList = new SelectList(conceptsList, "Id", "ConceptName");
            var conceptTheme = new ConceptTheme_VM();
            if (Id > 0)
            {
                conceptTheme = _conceptThemeRepository.Get(Id,loginUserId);
                if (conceptTheme == null)
                {
                    return RedirectToAction("List", "ConceptTheme");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(conceptTheme);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(conceptTheme);
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
            var result = _conceptThemeRepository.GetList(loginUserId);

            if (result.Any())
            {
                recordsTotal = result.Count;

                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.ConceptName.ToLower().Contains(search.ToLower())).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ConceptName).ToList() : result.OrderBy(p => p.ConceptName).ToList();
                    break;
                default:
                    result = result.OrderByDescending(p => p.ConceptId).ToList();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveAsync(ConceptTheme_VM conceptTheme_VM)
        {
            if (conceptTheme_VM != null)
            {
                if (conceptTheme_VM.LogoImage != null && conceptTheme_VM.LogoImage.Length > 0)
                {
                    conceptTheme_VM.LogoName = await Helper.FileUploadAsync(path, conceptTheme_VM.LogoImage);
                }

                if (conceptTheme_VM.FeedBackIcon != null && conceptTheme_VM.FeedBackIcon.Length > 0)
                {
                    conceptTheme_VM.FeedBackIconName = await Helper.FileUploadAsync(path, conceptTheme_VM.FeedBackIcon);
                }

                if (conceptTheme_VM.Id > 0)
                {
                    if (_conceptThemeRepository.Update(conceptTheme_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.conceptThemeUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.conceptThemeUpdateError;
                    }
                }
                else
                {
                    if (_conceptThemeRepository.Add(conceptTheme_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.conceptThemeAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.conceptThemeAddedError;
                    }
                }
            }
            return RedirectToAction("List", "ConceptTheme");
        }

        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            var result = _conceptThemeRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.conceptThemeDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.conceptThemeDeleted;
            }
            return Ok(response);
        }

        [HttpGet]
        public IActionResult CheckUniqueTheme(int conceptId)
        {
            var theme = _conceptThemeRepository.Get(conceptId, loginUserId);
            return Ok(theme != null ? false: true);
        }
    }
}

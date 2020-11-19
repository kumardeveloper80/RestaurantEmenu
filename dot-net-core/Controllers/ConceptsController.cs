using System;
using System.Linq;
using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EMenuApplication.Controllers
{
    [AuthLog]
    [AllowAnonymous]
    public class ConceptsController : BaseController
    {
        private readonly IConceptsRepository _conceptsRepository;
        private readonly IClientRepository _clientRepository;
        public ConceptsController(IConceptsRepository conceptsRepository, IClientRepository clientRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _conceptsRepository = conceptsRepository;
            _clientRepository = clientRepository;
        }

        public IActionResult Index(int Id = 0)
        {
            ViewBag.Clients = new SelectList(_clientRepository.GetList(loginUserId).Where(x => x.Status).ToList(), "Id", "CompanyName");
            var concepts = new Concepts_VM();
            if (Id > 0)
            {
                concepts = _conceptsRepository.Get(Id, loginUserId);
                if (concepts == null)
                {
                    return RedirectToAction("List", "Concepts");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(concepts);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(concepts);
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
            var result = _conceptsRepository.GetList(loginUserId);

            if (result.Any())
            {
                recordsTotal = result.Count;
                if (param.status == Helper.Active)
                {
                    result = result.Where(x => x.Active).ToList();
                }
                else if (param.status == Helper.Inactive)
                {
                    result = result.Where(x => !x.Active).ToList();
                }

                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.ConceptName.ToLower().Contains(search.ToLower())
                    || x.ClientName.ToLower().Contains(search.ToLower())).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ConceptName).ToList() : result.OrderBy(p => p.ConceptName).ToList();
                    break;
                case "1":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.ClientName).ToList() : result.OrderBy(p => p.ClientName).ToList();
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
            var result = _conceptsRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.conceptDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.conceptDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Concepts_VM concepts_VM)
        {
            if (concepts_VM != null)
            {
                if (concepts_VM.Id > 0)
                {
                    if (_conceptsRepository.Update(concepts_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.conceptUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.conceptUpdateError;
                    }
                }
                else
                {
                    if (_conceptsRepository.Add(concepts_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.conceptAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.conceptAddedError;
                    }
                }
            }
            return RedirectToAction("List", "Concepts");
        }

        [HttpGet]
        public IActionResult CheckConceptName(int id, string name, int clientId)
        {
            return Ok(_conceptsRepository.IsUniqueConceptName(id, name, clientId, loginUserId));
        }
    }
}

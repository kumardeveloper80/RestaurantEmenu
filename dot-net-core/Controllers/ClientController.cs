using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EMenuApplication.Controllers
{
    [AuthLog]
    [AllowAnonymous]
    public class ClientController : BaseController
    {
        private readonly IClientRepository _clientRepository;
        public ClientController(IClientRepository clientRepository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _clientRepository = clientRepository;
        }

        public IActionResult Index(int Id = 0)
        {
            var client = new Client_VM();
            if (Id > 0)
            {
                client = _clientRepository.Get(Id, this.loginUserId);
                if (client == null)
                {
                    return RedirectToAction("List", "Client");
                }
                else
                {
                    ViewData["Title"] = "Edit";
                    return View(client);
                }
            }
            else
            {
                ViewData["Title"] = "Add";
                return View(client);
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
            var result = _clientRepository.GetList(this.loginUserId);

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
                    result = result.Where(x => x.FirstName.ToLower().Contains(search.ToLower())
                        || x.LastName.ToLower().Contains(search.ToLower())
                        || x.CompanyName.ToLower().Contains(search.ToLower())
                        || x.EmailAddress.ToLower().Contains(search.ToLower())
                        || x.PhoneNo.ToLower().Contains(search.ToLower())
                        ).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.FirstName).ToList() : result.OrderBy(p => p.FirstName).ToList();
                    break;
                case "1":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.LastName).ToList() : result.OrderBy(p => p.LastName).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.CompanyName).ToList() : result.OrderBy(p => p.CompanyName).ToList();
                    break;
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.EmailAddress).ToList() : result.OrderBy(p => p.EmailAddress).ToList();
                    break;
                case "4":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.PhoneNo).ToList() : result.OrderBy(p => p.PhoneNo).ToList();
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
        [AllowAnonymous]
        public IActionResult Delete(int Id)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            var result = _clientRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.clientDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.clientDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Client_VM client_VM)
        {
            if (client_VM != null)
            {
                if (client_VM.Id > 0)
                {
                    if (_clientRepository.Update(client_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.clientUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.clientUpdateError;
                    }
                }
                else
                {
                    if (_clientRepository.Add(client_VM, this.loginUserId) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.clientAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.clientAddedError;
                    }
                }
            }
            return RedirectToAction("List", "Client");
        }

        [HttpGet]
        public IActionResult CheckUniqueCompanyName(int id, string companyName)
        {
            return Ok(_clientRepository.IsUniqueCompanyName(id, companyName, this.loginUserId));
        }

        [HttpGet]
        public IActionResult CheckUniqueEmail(int id, string email)
        {
            return Ok(_clientRepository.IsUniqueUniqueEmail(id, email, this.loginUserId));
        }
    }
}

using System;
using System.Linq;
using System.Threading.Tasks;
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
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IStoresRespository _storesRespository;
        public UserController(IUserRepository userRepository, IClientRepository clientRepository, IStoresRespository storesRespository, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _storesRespository = storesRespository;
        }

        public IActionResult Index(int Id = 0)
        {
            if (isSuperAdmin)
            {
                ViewBag.Clients = new SelectList(_clientRepository.GetList(loginUserId).Where(x => x.Status).ToList(), "Id", "CompanyName");
            }

            if (isAdmin)
            {
                ViewBag.StoresList = new SelectList(_storesRespository.GetStoresByUserId(loginUserId).ToList(), "Id", "StoreName");
            }

            if (Id > 0)
            {
                var user = _userRepository.Get(Id, isSuperAdmin);
                if (user == null)
                {
                    return RedirectToAction("List", "User");
                }
                ViewData["Title"] = "Edit";
                return View(user);
            }
            else
            {
                ViewData["Title"] = "Add";
                return View();
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
            var result = _userRepository.GetList(loginUserId, isSuperAdmin);

            if (result.Any())
            {
                recordsTotal = result.Count;
                if (!string.IsNullOrEmpty(param.search.value))
                {
                    var search = param.search.value;
                    result = result.Where(x => x.FullName.ToLower().Contains(search.ToLower())
                        || x.Email.ToLower().Contains(search.ToLower())
                        || x.UserName.ToLower().Contains(search.ToLower())
                        || x.ClientName.ToLower().Contains(search.ToLower())
                        || x.Phone.ToLower().Contains(search.ToLower())).ToList();
                }
            }

            string order = Convert.ToString(param.order[0].column);
            string orderDir = param.order[0].dir;
            switch (order)
            {
                case "0":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.FullName).ToList() : result.OrderBy(p => p.FullName).ToList();
                    break;
                case "1":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.UserName).ToList() : result.OrderBy(p => p.UserName).ToList();
                    break;
                case "2":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Email).ToList() : result.OrderBy(p => p.Email).ToList();
                    break;
                case "3":
                    result = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? result.OrderByDescending(p => p.Phone).ToList() : result.OrderBy(p => p.Phone).ToList();
                    break;
                case "4":
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
        [AllowAnonymous]
        public IActionResult Delete(int Id)
        {
            CommonResponse<int> response = new CommonResponse<int>();
            var result = _userRepository.Delete(Id, this.loginUserId);
            if (result == 0)
            {
                response.status = Helper.failure_code;
                response.message = Message.userDeletedError;
            }
            else
            {
                response.status = Helper.success_code;
                response.message = Message.userDeleted;
            }
            return Ok(response);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(User_VM user_VM)
        {
            if (user_VM != null)
            {
                if (isAdmin)
                {
                    user_VM.ClientId = clientId;
                }

                if (isSuperAdmin)
                {
                    user_VM.IsAllowVoucherApprovalPermission = true;
                    user_VM.IsAllowVoucherIssuancePermission = true;
                }

                if (user_VM.Id > 0)
                {
                    if (user_VM.ProfilePicture != null && user_VM.ProfilePicture.Length > 0)
                    {
                        user_VM.FileName = await Helper.FileUploadAsync(path, user_VM.ProfilePicture);
                    }

                    if (_userRepository.Update(user_VM, this.loginUserId, isSuperAdmin) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.userUpdated;
                    }
                    else
                    {
                        TempData["Message"] = Message.userUpdatedError;
                    }
                }
                else
                {
                    if (_userRepository.Add(user_VM, this.loginUserId, isSuperAdmin) > 0)
                    {
                        TempData["Status"] = Helper.success_code;
                        TempData["Message"] = Message.userAdded;
                    }
                    else
                    {
                        TempData["Message"] = Message.userAddedError;
                    }
                }
            }
            return RedirectToAction("List", "User");
        }

        [HttpGet]
        public IActionResult CheckUniqueEmail(int id, string email)
        {
            return Ok(_userRepository.IsUniqueEmail(id, email));
        }

        [HttpGet]
        public IActionResult CheckUniqueUserName(int id, string username)
        {
            return Ok(_userRepository.IsUniqueUserName(id, username));
        }

        [HttpGet]
        public IActionResult CheckClientAssign(int id, int clientId)
        {
            if (clientId > 0)
            {
                return Ok(_userRepository.IsClientAssign(id, clientId));
            }
            return Ok(true);
        }
    }
}

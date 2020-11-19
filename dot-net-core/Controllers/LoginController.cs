using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;

namespace EMenuApplication.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        public LoginController(IUserRepository userRepository, IConfiguration configuration, ILogger<LoginController> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null && userId > 0)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Authentication(Login_VM model)
        {
            _logger.LogInformation("Authentication::Start");
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Model::Done");
                var result = _userRepository.Login(model);
                if (result != null)
                {
                    _logger.LogInformation("User::Valid");
                    HttpContext.Session.SetString("fullname", result.FullName);
                    HttpContext.Session.SetInt32("UserId", result.UserId);
                    HttpContext.Session.SetInt32("IsAdmin", result.IsAdmin ? 1 : 0);
                    HttpContext.Session.SetInt32("SelectedConceptId", 0);
                    HttpContext.Session.SetInt32("ClientId", result.ClientId);
                    HttpContext.Session.SetInt32("HasVoucherIssuancePermission", result.IsAllowVoucherIssuancePermission ? 1 : 0);
                    HttpContext.Session.SetInt32("HasAllowVoucherApprovalPermission", result.IsAllowVoucherApprovalPermission ? 1 : 0);
                    string pic = result.ProfilePic;
                    var folderName = "/" + _configuration.GetValue<string>("UploadFolder");
                    var path = Path.Combine(folderName, pic);
                    HttpContext.Session.SetString("imgpath", path);


                    return RedirectToActionPermanent("Index", "Dashboard");
                }
                _logger.LogInformation("User::InValid");
                ModelState.AddModelError("", Message.invalidLogin);
            }
            _logger.LogInformation("Authentication::End");
            return View("Index", model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Login");
        }
    }
}
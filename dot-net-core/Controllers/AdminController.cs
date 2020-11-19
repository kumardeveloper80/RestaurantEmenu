using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EMenuApplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        public AdminController(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null && userId > 0)
            {
                return RedirectToAction("List", "Client");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Authentication(Login_VM model)
        {
            if (ModelState.IsValid)
            {
                var result = _userRepository.SuperAdminLogin(model);
                if (result != null)
                {
                    HttpContext.Session.SetString("fullname", result.FullName);
                    HttpContext.Session.SetInt32("UserId", result.UserId);
                    HttpContext.Session.SetInt32("IsAdmin", 0);
                    HttpContext.Session.SetInt32("IsSuperAdmin", 1);
                    string pic = result.ProfilePic;
                    var folderName = "/" + _configuration.GetValue<string>("UploadFolder");
                    var path = Path.Combine(folderName, pic);
                    HttpContext.Session.SetString("imgpath", path);
                    return RedirectToActionPermanent("List", "Client");
                }
                ModelState.AddModelError("", Message.invalidLogin);
            }
            return View("Index", model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Admin");
        }
    }
}

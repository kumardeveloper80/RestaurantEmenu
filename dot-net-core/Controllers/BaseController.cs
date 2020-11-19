using EMenuApplication.Repository.Interface;
using EMenuApplication.Utility;
using EMenuApplication.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace EMenuApplication.Controllers
{
    public class BaseController : Controller
    {
        public int loginUserId { get; set; }
        public bool isAdmin { get; set; }
        public bool isSuperAdmin { get; set; }
        public List<Concepts_VM> conceptsList { get; set; }
        public string folderName { get; set; }
        public string path { get; set; }
        public string QRCodeUrl { get; set; }
        public int SelectedConceptId { get; set; }
        public int clientId { get; set; }

        public bool hasVoucherApprovalPermission { get; set; }
        public bool hasVoucherIssuancePermission { get; set; }

        public readonly IConfiguration _configuration;
        public readonly IWebHostEnvironment _hostingEnvironment;
        public readonly IServiceProvider _serviceProvider;
        private readonly IConceptsRepository _conceptsRepository;
        public BaseController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _conceptsRepository = (IConceptsRepository)this._serviceProvider.GetService(typeof(IConceptsRepository));
            _configuration = (IConfiguration)this._serviceProvider.GetService(typeof(IConfiguration));
            _hostingEnvironment = (IWebHostEnvironment)this._serviceProvider.GetService(typeof(IWebHostEnvironment));

            loginUserId = AppContext.Current.Session.GetInt32("UserId") != null ? AppContext.Current.Session.GetInt32("UserId").Value : 0;
            isAdmin = AppContext.Current.Session.GetInt32("IsAdmin") == 1 ? true : false;
            isSuperAdmin = AppContext.Current.Session.GetInt32("IsSuperAdmin") == 1 ? true : false;
            clientId = AppContext.Current.Session.GetInt32("ClientId") != null ? AppContext.Current.Session.GetInt32("ClientId").Value : 0;
            folderName = this._configuration.GetValue<string>("UploadFolder");
            path = Path.Combine(this._hostingEnvironment.WebRootPath, folderName);
            QRCodeUrl = this._configuration.GetValue<string>("QRCodeURL");
            SelectedConceptId = AppContext.Current.Session.GetInt32("SelectedConceptId") != null ? AppContext.Current.Session.GetInt32("SelectedConceptId").Value : 0;
            conceptsList = _conceptsRepository.GetConceptsPermissionList(loginUserId);
            AppContext.Current.Session.SetComplexData("conceptsList", conceptsList);
            //hasVoucherApprovalPermission = AppContext.Current.Session.GetInt32("HasAllowVoucherApprovalPermission") == 1 ? true : false;
            //hasVoucherIssuancePermission = AppContext.Current.Session.GetInt32("HasVoucherIssuancePermission") == 1 ? true : false;

            if (AppContext.Current.Session.GetInt32("HasAllowVoucherApprovalPermission") == 1 || isAdmin)
            {
                hasVoucherApprovalPermission = true;
            }

            if (AppContext.Current.Session.GetInt32("HasVoucherIssuancePermission") == 1 || isAdmin)
            {
                hasVoucherIssuancePermission = true;
            }
        }
        override
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controllerName = context.RouteData.Values["controller"].ToString();
            if (this.loginUserId == 0)
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(
                new
                {
                    controller = "Login",
                    action = "Index"
                }));
            }
            else if (isSuperAdmin)
            {
                if (controllerName != "Client" && controllerName != "User" && controllerName != "Concepts" && controllerName != "Stores" && controllerName != "Region" && controllerName != "Country")
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Client",
                        action = "List"
                    }));
                }
            }
            else if (!isAdmin && (controllerName == "Currency" || controllerName == "VoucherSetup" || controllerName == "VoucherReasonCategory" || controllerName == "VoucherReasonSubCategory"))
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Dashboard",
                    action = "Index"
                }));
            }
        }

        [HttpPost]
        public IActionResult ChangeConcept(int id)
        {
            HttpContext.Session.SetInt32("SelectedConceptId", id);
            return Ok(true);
        }
    }
}

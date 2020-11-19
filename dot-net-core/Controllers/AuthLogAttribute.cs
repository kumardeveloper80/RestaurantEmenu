using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EMenuApplication.Controllers
{
    public class AuthLogAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var controllerName = context.RouteData.Values["controller"].ToString();
            var isAdmin = AppContext.Current.Session.GetInt32("IsAdmin");
            var isSuperAdmin = AppContext.Current.Session.GetInt32("IsSuperAdmin");
            if (isSuperAdmin == 1 || isAdmin == 1)
            {
                if(isAdmin == 1 && controllerName != "User")
                {
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                    {
                        controller = "Dashboard",
                        action = "Index"
                    }));
                }
            }
            else
            {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    controller = "Dashboard",
                    action = "Index"
                }));
            }
        }
    }
}

using PhysicManagement.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhysicManagement.Controllers
{
    [Authorization()]
    public class BaseController : Controller
    {
        
    }
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            string RoleTypeCookieName = "RoleType";
            string RoleType = Common.Cookie.ReadCookie(RoleTypeCookieName)?.ToLower();
            switch (RoleType)
            {
                case "doctor":
                    {
                        var UserData = DoctorService.IsAuthenticated();
                        if (UserData == null || UserData.IsActive == false)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(new
                                {
                                    action = "Login",
                                    controller = "Account",
                                    area = "",
                                    data = filterContext.HttpContext.Request.Url.AbsoluteUri
                                }));
                        }
                    }
                    break;
                case "resident":
                    {
                        var UserData = ResidentService.IsAuthenticated();
                        if (UserData == null || UserData.IsActive == false)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(new
                                {
                                    action = "Login",
                                    controller = "Account",
                                    area = "",
                                    data = filterContext.HttpContext.Request.Url.AbsoluteUri
                                }));
                        }
                    }
                    break;
                case "physicuser":
                    {
                        var UserData = PhysicUserService.IsAuthenticated();
                        if (UserData == null || UserData.IsActive == false)
                        {
                            filterContext.Result = new RedirectToRouteResult(
                                new RouteValueDictionary(new
                                {
                                    action = "Login",
                                    controller = "Account",
                                    area = "",
                                    data = filterContext.HttpContext.Request.Url.AbsoluteUri
                                }));
                        }
                        break;
                    }
                default: 
                    {
                        filterContext.Result = new RedirectToRouteResult(
                                   new RouteValueDictionary(new
                                   {
                                       action = "Login",
                                       controller = "Account",
                                       area = "",
                                       data = filterContext.HttpContext.Request.Url.AbsoluteUri
                                   }));
                    }
                    break;
            }
          

        }
    }
}
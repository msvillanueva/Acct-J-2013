using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Web.Core.DataAccess;

namespace Web.Core
{
    public class ActionFilter : IActionFilter, IResultFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //throw new System.NotImplementedException();
            var controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            if (controller == "Login")
                return;

            var actionType = ActionMethodInfo(filterContext.ActionDescriptor.ActionName, filterContext.Controller.GetType()).ReturnType.Name;

            bool invalidAccess = false;
            var returnUrl = "";
            var redirectAction = "";

            if (actionType == "ActionResult")
            {
                if (filterContext.HttpContext.Request.UrlReferrer == null)
                    returnUrl = filterContext.HttpContext.Request.Url.AbsoluteUri;
                else
                    returnUrl = filterContext.HttpContext.Request.UrlReferrer.AbsoluteUri;

                if (!UserSession.IsLoggedIn)
                {
                    invalidAccess = true;
                    redirectAction = "Invoke";
                }
                else
                {
                    if (!UserSession.AllowedControllers.Contains(controller.Trim().ToUpper()))
                    {
                        invalidAccess = true;
                        redirectAction = "Index";
                    }
                }
            }
            else if (actionType == "ViewResult")
            {
                if (!UserSession.IsLoggedIn)
                    return;
            }

            if (invalidAccess)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(
                    new
                    {
                        controller = "Login",
                        action = redirectAction,
                        url = returnUrl
                    }));
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //throw new System.NotImplementedException();
        }

        public void OnResultExecuting(ResultExecutingContext filterContext)
        {
            //throw new System.NotImplementedException();
        }

        public void OnResultExecuted(ResultExecutedContext filterContext)
        {
            //throw new System.NotImplementedException();
        }

        public MethodInfo ActionMethodInfo(string actionName, Type controllerType)
        {
            return controllerType.GetMethod(actionName);
        }

    }
}
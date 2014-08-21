using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Web.Core.DataAccess;
using Web.Core;

namespace Web.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Login/

        public ActionResult Index(string url="")
        {
            if (UserSession.IsLoggedIn)
                return RedirectToAction("Index", "Home");
            ViewBag.Url = url;
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Login([DataSourceRequest] DataSourceRequest request, string username, string password)
        {
            var data = "";
            var ip = Request.UserHostAddress.Replace(":", " ");
            try
            {
                var user = LUser.GetUser(username, password);
                if (user != null)
                {
                    UserSession.User = user;
                    UserSession.LoggedIn = true;
                    UserSession.Username = user.Username;
                    UserSession.Role = (Enumerations.Role) user.Role;
                    UserSession.UserId = user.ID;
                    UserSession.AllowedControllers = LActionRoute.GetAllowedControllers(UserSession.Role);
                    UserSession.AllowedRoutes = LActionRoute.GetAllowedRoutes(UserSession.Role);
                    LSystemLog.Insert(UserSession.UserId, Enumerations.ActionType.Login, Enumerations.ActionTable.User, "User Login");
                    LUser.SetLoginDate(user.ID);
                }
                else
                {
                    throw new Exception("Login Failed!");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { data }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Logout()
        {
            if (UserSession.IsLoggedIn)
                LSystemLog.Insert(UserSession.UserId, Enumerations.ActionType.Logout, Enumerations.ActionTable.User, "System Logout");
            WebContext.ClearSession();
            return RedirectToAction("Index", "Login");
        }

        public ActionResult Invoke(string url = "")
        {
            ViewBag.Url = url;
            return View();
        }
    }
}

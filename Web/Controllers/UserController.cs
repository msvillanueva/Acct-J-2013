using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Web.Core.DataAccess;
using Web.Models;
using Web.Core;

namespace Web.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            ViewBag.Title = "Manage Users";
            ViewBag.SubTitle = "Add, edit, delete and assign roles to users.";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = LUser.GetUsers(UserSession.UserId);
            return Json(data.ToDataSourceResult(request));
        }

        public ActionResult Update(int id = 0, string mode = "CREATE")
        {
            ViewBag.Mode = mode.ToUpper().Trim();
            var roles = Enumerations.GetList(typeof(Enumerations.Role));
            ViewBag.Roles = roles.ToList()
                .Select(a => new SelectListItem()
                {
                    Value = a.Key.ToString(),
                    Text = a.Value
                })
                .ToList();
            var outUser = new CUser();
            outUser.IsActive = true;
            var _user = LUser.GetUser(id);
            if (_user != null)
                outUser = _user;
            return View(outUser);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Insert([DataSourceRequest] DataSourceRequest request, CUser user)
        {
            if (user != null && ModelState.IsValid)
            {
                try
                {
                    LUser.Create(user);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Insert, Core.Enumerations.ActionTable.User, user.Username);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid user data.");
            }
            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Update([DataSourceRequest] DataSourceRequest request, CUser user)
        {
            if (user != null && ModelState.IsValid)
            {
                try
                {
                    var _user = LUser.GetUser(user.ID);
                    LUser.Update(user);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.User, _user.Username);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid user data.");
            }
            return Json(new[] { user }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Delete([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var _user = LUser.GetUser(id);
                LUser.Delete(_user.ID);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.User, _user.Username);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }

    }
}

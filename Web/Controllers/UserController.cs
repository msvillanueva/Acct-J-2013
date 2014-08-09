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
            ViewBag.AllowEncAcctEntries = LRolePermission.GetRolePermission(Enumerations.Role.Encoder, LActionRoute.GetActionRoute("Index", "AccountEntry")) != null;
            ViewBag.AllowEncPayees = LRolePermission.GetRolePermission(Enumerations.Role.Encoder, LActionRoute.GetActionRoute("Index", "Payee")) != null;
            return View();
        }

        public ActionResult ChangePassword()
        {
            ViewBag.Title = "Change Password";
            ViewBag.SubTitle = "Replace your existing password.";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = LUser.GetUsers();
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
                if (UserSession.UserId == _user.ID)
                    throw new Exception("You cannot delete your won account.");
                LUser.Delete(_user.ID);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.User, _user.Username);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_ChangePassword([DataSourceRequest] DataSourceRequest request, string password, string oldpassword)
        {
            try
            {
                var _user = LUser.GetUser(UserSession.UserId);
                if (_user.Password != Encryption.Encrypt(oldpassword))
                    throw new Exception("Old password is invalid.");
                _user.Password = password;
                LUser.Update(_user);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.User, _user.Username + " password");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { 1 }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ToggleEncoderAccountEntriesMng([DataSourceRequest] DataSourceRequest request, bool allowed)
        {
            try
            {
                if (allowed)
                {
                    LRolePermission.Create(Enumerations.Role.Encoder, LActionRoute.GetActionRoute("Index", "AccountEntry"));
                }
                else
                {
                    LRolePermission.Delete(Enumerations.Role.Encoder, LActionRoute.GetActionRoute("Index", "AccountEntry"));
                }
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.ChangePermission, Core.Enumerations.ActionTable.RolePermission, (allowed ? "Allow" : "Restrict") + " encoder to manage account entries");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { "" }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ToggleEncoderPayeesMng([DataSourceRequest] DataSourceRequest request, bool allowed)
        {
            try
            {
                if (allowed)
                {
                    LRolePermission.Create(Enumerations.Role.Encoder, LActionRoute.GetActionRoute("Index", "Payee"));
                }
                else
                {
                    LRolePermission.Delete(Enumerations.Role.Encoder, LActionRoute.GetActionRoute("Index", "Payee"));
                }
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.ChangePermission, Core.Enumerations.ActionTable.RolePermission, (allowed ? "Allow" : "Restrict") + " encoder to manage payees");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { "" }.ToDataSourceResult(request, ModelState));
        }

    }
}

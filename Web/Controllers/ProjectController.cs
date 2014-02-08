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
    public class ProjectController : Controller
    {
        //
        // GET: /Project/

        public ActionResult Index()
        {
            ViewBag.Title = "Manage Projects";
            ViewBag.SubTitle = "Add, edit and delete project data.";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = LProject.GetProjects();
            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Insert([DataSourceRequest] DataSourceRequest request, CProject proj)
        {
            if (proj != null && ModelState.IsValid)
            {
                try
                {
                    if (proj.Code == null || proj.Code.Trim() == "")
                        throw new Exception("Project code is required");
                    if (proj.Name == null || proj.Name.Trim() == "")
                        throw new Exception("Project name is required");
                    LProject.Create(proj);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Insert, Core.Enumerations.ActionTable.Project, proj.Code);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { proj }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Update([DataSourceRequest] DataSourceRequest request, CProject proj)
        {
            if (proj != null && ModelState.IsValid)
            {
                try
                {
                    if (proj.Code == null || proj.Code.Trim() == "")
                        throw new Exception("Project code is required");
                    if (proj.Name == null || proj.Name.Trim() == "")
                        throw new Exception("Project name is required");
                    LProject.Update(proj);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.Project, proj.Code);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { proj }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Delete([DataSourceRequest] DataSourceRequest request, CProject proj)
        {
            if (proj != null && ModelState.IsValid)
            {
                try
                {
                    var _p = LProject.GetProject(proj.ID);
                    LProject.Delete(proj.ID);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.Project, _p.Code);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { proj }.ToDataSourceResult(request, ModelState));
        }

    }
}

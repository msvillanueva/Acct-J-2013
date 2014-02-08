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
    public class ReportSettingsController : Controller
    {
        //
        // GET: /ReportSettings/

        public ActionResult Index()
        {
            ViewBag.Title = "Report Settings";
            ViewBag.SubTitle = "Set report permissions to encoders.";
            ViewBag.RoleID = 3;
            ViewBag.Permissions = LReportPermission.GetReportPermissions(3);
            return View(LReportPermission.GetReports());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, int reportid, int roleid, bool allowed)
        {
            var report = LReportPermission.GetReport(reportid);
            if (report != null)
            {
                try
                {
                    LReportPermission.Update(roleid, reportid, allowed);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.ChangePermission, Core.Enumerations.ActionTable.Report, ((Enumerations.Role)roleid).ToString());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid report ID.");
            }
            return Json(new[] { report }.ToDataSourceResult(request, ModelState));
        }

    }
}

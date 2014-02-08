using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Web.Core.DataAccess;
using Web.Models;

namespace Web.Controllers
{
    public class LogsController : Controller
    {
        //
        // GET: /Logs/

        public ActionResult Index()
        {
            ViewBag.Title = "Audit Trail";
            ViewBag.SubTitle = "View logs and user activities.";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_GetLogs([DataSourceRequest] DataSourceRequest request)
        {
            if (!UserSession.IsLoggedIn)
                return null;
            var data = LSystemLog.GetLogs();
            return Json(data.ToDataSourceResult(request));
        }

    }
}

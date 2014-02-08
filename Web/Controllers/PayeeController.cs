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
    public class PayeeController : Controller
    {
        //
        // GET: /Payee/

        public ActionResult Index()
        {
            ViewBag.Title = "Manage Payees";
            ViewBag.SubTitle = "Add, edit and delete payee data.";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = LPayee.GeyPayees();
            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Insert([DataSourceRequest] DataSourceRequest request, CPayee payee)
        {
            if (payee != null && ModelState.IsValid)
            {
                try
                {
                    LPayee.Create(payee);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Insert, Core.Enumerations.ActionTable.Payee, payee.Name);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { payee }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Update([DataSourceRequest] DataSourceRequest request, CPayee payee)
        {
            if (payee != null && ModelState.IsValid)
            {
                try
                {
                    LPayee.Update(payee);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.Payee, payee.Name);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { payee }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Delete([DataSourceRequest] DataSourceRequest request, CPayee payee)
        {
            if (payee != null && ModelState.IsValid)
            {
                try
                {
                    var _payee = LPayee.GeyPayee(payee.ID);
                    LPayee.Delete(payee.ID);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.Payee, _payee.Name);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { payee }.ToDataSourceResult(request, ModelState));
        }

    }
}

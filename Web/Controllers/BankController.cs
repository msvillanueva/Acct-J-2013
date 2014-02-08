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
    public class BankController : Controller
    {
        //
        // GET: /Bank/

        public ActionResult Index()
        {
            ViewBag.Title = "Manage Banks";
            ViewBag.SubTitle = "Add, edit and delete bank data.";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Read([DataSourceRequest] DataSourceRequest request)
        {

            var data = LBank.GetBanks();
            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Insert([DataSourceRequest] DataSourceRequest request, CBank bank)
        {
            if (bank != null && ModelState.IsValid)
            {
                try
                {
                    LBank.Create(bank.Code, bank.Name);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Insert, Core.Enumerations.ActionTable.Bank, bank.Code);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { bank }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Update([DataSourceRequest] DataSourceRequest request, CBank bank)
        {
            if (bank != null && ModelState.IsValid)
            {
                try
                {
                    LBank.Update(bank.ID, bank.Code, bank.Name);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.Bank, bank.Code);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { bank }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Delete([DataSourceRequest] DataSourceRequest request, CBank bank)
        {
            if (bank != null && ModelState.IsValid)
            {
                try
                {
                    var _bank = LBank.GetBank(bank.ID);
                    LBank.Delete(bank.ID);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.Bank, _bank.Code);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { bank }.ToDataSourceResult(request, ModelState));
        }

    }
}

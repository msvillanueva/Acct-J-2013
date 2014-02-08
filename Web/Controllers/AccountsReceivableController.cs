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
    public class AccountsReceivableController : Controller
    {
        //
        // GET: /AccountsReceivable/

        public ActionResult Index()
        {
            ViewBag.Title = "Accounts Receivables";
            ViewBag.SubTitle = "Create, Edit and remove accounts receivables";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = LSalesInvoice.GetSalesInvoices(UserSession.Role == Enumerations.Role.SuperUser);
            return Json(data.ToDataSourceResult(request));
        }

        public ActionResult Update(int id = 0, string mode = "CREATE")
        {
            var projects = LProject.GetProjects();
            ViewBag.ProjectLookUp = projects.ToList();
            ViewBag.Projects = projects.ToList()
                .Select(a => new SelectListItem()
                {
                    Value = a.ID.ToString(),
                    Text = a.Name
                })
                .ToList();

            ViewBag.Mode = mode.ToUpper().Trim();
            var outInvoice = new CSalesInvoice();
            outInvoice.DateAdded = DateTime.Now;
            var _invoice = LSalesInvoice.GetSalesInvoice(id);
            if (_invoice != null)
                outInvoice = _invoice;
            return View(outInvoice);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Insert([DataSourceRequest] DataSourceRequest request, CSalesInvoice invoice)
        {
            if (invoice != null && ModelState.IsValid)
            {
                try
                {
                    invoice.AddedByID = UserSession.UserId;
                    invoice.ID = LSalesInvoice.Create(invoice);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Insert, Core.Enumerations.ActionTable.SalesInvoice, invoice.ID.ToString());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid invoice data.");
            }
            return Json(new[] { invoice }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Update([DataSourceRequest] DataSourceRequest request, CSalesInvoice invoice)
        {
            if (invoice != null && ModelState.IsValid)
            {
                try
                {
                    LSalesInvoice.Update(invoice);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.SalesInvoice, invoice.ID.ToString());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid invoice data.");
            }
            return Json(new[] { invoice }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Delete([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var _invoice = LSalesInvoice.GetSalesInvoice(id);
                LSalesInvoice.Delete(_invoice.ID);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.SalesInvoice, id.ToString());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Invoice(int invoiceID)
        {
            var urls = new List<CActionRoute>();
            urls.Add(new CActionRoute() { Action = "Index", Controller = "AccountsReceivable", Description = "Accounts Receivable" });
            ViewBag.ParentUrl = urls;
            ViewBag.Title = "Sales Invoice Details";
            ViewBag.SubTitle = "Manage entries";
            var invoice = LSalesInvoice.GetSalesInvoice(invoiceID);
            invoice.Entries = new List<CSalesInvoiceEntry>();
            invoice.Entries = LSalesInvoiceEntry.GetEntries(invoice.ID);
            return View(invoice);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_ReadEntries([DataSourceRequest] DataSourceRequest request, int invoiceid)
        {
            var data = LSalesInvoiceEntry.GetEntries(invoiceid);
            return Json(data.ToDataSourceResult(request));
        }

        public ActionResult UpdateEntry(int invoiceid, int id = 0, string mode = "CREATE")
        {
            var accountEntries = LAccountEntry.GetEntries();
            ViewBag.AccountEntriyLookup = accountEntries.ToList();
            ViewBag.AccountEntries = accountEntries.ToList()
                .Select(a => new SelectListItem()
                {
                    Value = a.ID.ToString(),
                    Text = a.Title
                })
                .ToList();

            var projects = LProject.GetProjects();
            ViewBag.ProjectLookUp = projects.ToList();
            ViewBag.Projects = projects.ToList()
                .Select(a => new SelectListItem()
                {
                    Value = a.ID.ToString(),
                    Text = a.Name
                })
                .ToList();

            ViewBag.Mode = mode.ToUpper().Trim();
            var outEntry = new CSalesInvoiceEntry();
            var invoice = LSalesInvoice.GetSalesInvoice(invoiceid);
            outEntry.SalesInvoiceID = invoiceid;
            outEntry.ProjectID = invoice.ProjectID;
            outEntry.Quantity = 1;
            var _entry = LSalesInvoiceEntry.GetEntry(id);
            if (_entry != null)
                outEntry = _entry;
            return View(outEntry);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_InsertEntry([DataSourceRequest] DataSourceRequest request, CSalesInvoiceEntry entry)
        {
            if (entry != null && ModelState.IsValid)
            {
                try
                {
                    entry.AddedByID = UserSession.UserId;
                    var _entry = LSalesInvoiceEntry.GetEntry(LSalesInvoiceEntry.Create(entry));
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Insert, Core.Enumerations.ActionTable.SalesInvoiceEntry, "Invoice No. " + entry.SalesInvoiceID + ": " + entry.Articles);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid invoice data.");
            }
            return Json(new[] { entry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_UpdateEntry([DataSourceRequest] DataSourceRequest request, CSalesInvoiceEntry entry)
        {
            if (entry != null && ModelState.IsValid)
            {
                try
                {
                    LSalesInvoiceEntry.Update(entry);
                    var _entry = LSalesInvoiceEntry.GetEntry(entry.ID);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.SalesInvoiceEntry, "Invoice No. " + _entry.SalesInvoiceID + ": " + _entry.Articles);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid invoice data.");
            }
            return Json(new[] { entry }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_DeleteEntry([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var _entry = LSalesInvoiceEntry.GetEntry(id);
                LSalesInvoiceEntry.Delete(_entry.ID);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.SalesInvoiceEntry, "Invoice No. " + _entry.SalesInvoiceID + ": " + _entry.Articles);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult SubmitInvoice(int id)
        {
            return View(LSalesInvoice.GetSalesInvoice(id));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_SubmitInvoice([DataSourceRequest] DataSourceRequest request, CSalesInvoice invoice)
        {
            var _invoice = new CSalesInvoice();
            try
            {
                _invoice = LSalesInvoice.GetSalesInvoice(invoice.ID);
                _invoice.PWDDiscount = invoice.PWDDiscount;
                _invoice.VatExemptSales = invoice.VatExemptSales;
                _invoice.ZeroRatedSales = invoice.ZeroRatedSales;
                _invoice.ORNo = invoice.ORNo;
                LSalesInvoice.Submit(_invoice);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Submit, Core.Enumerations.ActionTable.SalesInvoice, "Invoice No. " + _invoice.ID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { _invoice }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_DenyInvoice([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var invoice = LSalesInvoice.GetSalesInvoice(id);
                LSalesInvoice.Deny(invoice.ID);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Deny, Core.Enumerations.ActionTable.SalesInvoice, "Invoice No. " + invoice.ID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }
    }
}

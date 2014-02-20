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
    public class AccountsPayableController : Controller
    {
        //
        // GET: /AccountsPayable/

        public ActionResult Index()
        {
            ViewBag.Title = "Accounts Payable";
            ViewBag.SubTitle = "Create, Edit and remove accounts payable";
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = LVoucher.GetVouchers(UserSession.Role == Enumerations.Role.SuperUser);
            return Json(data.ToDataSourceResult(request));
        }

        public ActionResult Update(int id = 0, string mode = "CREATE")
        {
            var payees = LPayee.GeyPayees();
            ViewBag.Payees = payees.ToList()
                .Select(a => new SelectListItem()
                {
                    Value = a.ID.ToString(),
                    Text = a.Name
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
            var outVoucher = new CVoucher();
            outVoucher.CheckDate = DateTime.Now;
            outVoucher.DateAdded = DateTime.Now;
            var _voucher = LVoucher.GetVoucher(id);
            if (_voucher != null)
                outVoucher = _voucher;
            return View(outVoucher);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Insert([DataSourceRequest] DataSourceRequest request, CVoucher voucher, string inputtax = "", decimal ewt = 0)
        {
            if (voucher != null && ModelState.IsValid)
            {
                try
                {
                    voucher.AddedByID = UserSession.UserId;
                    voucher.ID = LVoucher.Create(voucher);
                    decimal netOfVat;
                    if (inputtax == "true")
                    {
                        var inputTaxEntry = LAccountEntry.GetEntry(Constants.CodeInputTax());
                        if (inputTaxEntry != null)
                        {
                            //var inputTax = voucher.CheckAmount - (voucher.CheckAmount / (Constants.VATPercent() + 1));
                            netOfVat = (decimal)(voucher.CheckAmount / (Constants.VATPercent() + 1));
                            var inputTax = netOfVat * Constants.VATPercent();
                            var inputTaxVEntry = new CVoucherEntry()
                            {
                                VoucherID = voucher.ID,
                                AccountEntryID = inputTaxEntry.ID,
                                AddedByID = UserSession.UserId,
                                ProjectID = voucher.ProjectID,
                                Debit = Decimal.Round((decimal)inputTax,2),
                                Credit = 0
                            };
                            LVoucherEntry.Create(inputTaxVEntry);
                        }
                    }
                    if (ewt != 0)
                    {
                        var ewtEntry = LAccountEntry.GetEntry(Constants.CodeExpandedTax());
                        if (ewtEntry != null)
                        {
                            var ewtPercent = ewt / 100;
                            var ewtAmount = (voucher.CheckAmount / (inputtax == "true" ? (Constants.VATPercent() + 1) : 1)) * ewtPercent;
                            var ewtVEntry = new CVoucherEntry()
                            {
                                VoucherID = voucher.ID,
                                AccountEntryID = ewtEntry.ID,
                                AddedByID = UserSession.UserId,
                                ProjectID = voucher.ProjectID,
                                Credit = Decimal.Round((decimal)ewtAmount, 2),
                                Debit = 0
                            };
                            LVoucherEntry.Create(ewtVEntry);
                            voucher.CheckAmount = voucher.CheckAmount - Decimal.Round((decimal)ewtAmount, 2);
                            LVoucher.Update(voucher);
                        }
                    }
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Insert, Core.Enumerations.ActionTable.Voucher, voucher.ID.ToString());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid voucher data.");
            }
            return Json(new[] { voucher }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Update([DataSourceRequest] DataSourceRequest request, CVoucher voucher)
        {
            if (voucher != null && ModelState.IsValid)
            {
                try
                {
                    LVoucher.Update(voucher);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.Voucher, voucher.ID.ToString());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid voucher data.");
            }
            return Json(new[] { voucher }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Delete([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var _voucher = LVoucher.GetVoucher(id);
                LVoucher.Delete(_voucher.ID);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.Voucher, id.ToString());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }

        public ActionResult Voucher(int voucherID)
        {
            var urls = new List<CActionRoute>();
            urls.Add(new CActionRoute() { Action = "Index", Controller = "AccountsPayable", Description = "Accounts Payable" });
            ViewBag.ParentUrl = urls;
            ViewBag.Title = "Voucher Details";
            ViewBag.SubTitle = "Manage account entries";
            var voucher = LVoucher.GetVoucher(voucherID);
            voucher.Entries = new List<CVoucherEntry>();
            voucher.Entries = LVoucherEntry.GetEntries(voucher.ID);
            return View(voucher);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_ReadEntries([DataSourceRequest] DataSourceRequest request, int voucherid)
        {
            var data = LVoucherEntry.GetEntries(voucherid).OrderBy(a => a.Credit);
            return Json(data.ToDataSourceResult(request));
        }

        public ActionResult UpdateEntry(int voucherid, int id = 0, string mode = "CREATE")
        {
            var banks = LBank.GetBanks();
            ViewBag.Banks = banks.ToList()
                .Select(a => new SelectListItem()
                {
                    Value = a.ID.ToString(),
                    Text = a.Name
                })
                .ToList();

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
            var outEntry = new CVoucherEntry();
            var voucher = LVoucher.GetVoucher(voucherid);
            outEntry.VoucherID = voucherid;
            outEntry.ProjectID = voucher.ProjectID;
            var _entry = LVoucherEntry.GetEntry(id);
            if (_entry != null)
                outEntry = _entry;
            return View(outEntry);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_InsertEntry([DataSourceRequest] DataSourceRequest request, CVoucherEntry entry)
        {
            if (entry != null && ModelState.IsValid)
            {
                try
                {
                    entry.AddedByID = UserSession.UserId;
                    var _entry = LVoucherEntry.GetEntry(LVoucherEntry.Create(entry));
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Insert, Core.Enumerations.ActionTable.VoucherEntry, "Voucher No. " + entry.VoucherID + ": " + entry.AccountEntryName);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid voucher data.");
            }
            return Json(new[] { entry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_UpdateEntry([DataSourceRequest] DataSourceRequest request, CVoucherEntry entry)
        {
            if (entry != null && ModelState.IsValid)
            {
                try
                {
                    entry.AddedByID = UserSession.UserId;
                    LVoucherEntry.Update(entry);
                    var _entry = LVoucherEntry.GetEntry(entry.ID);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.Voucher, "Voucher No. " + _entry.VoucherID + ": " + _entry.AccountEntryName);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("data_error", "Invalid voucher data.");
            }
            return Json(new[] { entry }.ToDataSourceResult(request, ModelState));
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_DeleteEntry([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var _entry = LVoucherEntry.GetEntry(id);
                LVoucherEntry.Delete(_entry.ID);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.VoucherEntry, "Voucher No. " + _entry.VoucherID + ": " + _entry.AccountEntryName);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_SubmitVoucher([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var voucher = LVoucher.GetVoucher(id);
                voucher.Entries = new List<CVoucherEntry>();
                voucher.Entries = LVoucherEntry.GetEntries(voucher.ID);
                var sumDebit = voucher.Entries.Sum(a => a.Debit);
                var sumCredit = voucher.Entries.Sum(a => a.Credit);
                if (sumDebit != sumCredit)
                {
                    if (sumDebit > sumCredit)
                    {
                        throw new Exception("There is a discrepancy of " + string.Format("{0:n}", sumDebit - sumCredit) + " debit.");
                    }
                    else
                    {
                        throw new Exception("There is a discrepancy of " + string.Format("{0:n}", sumCredit - sumDebit) + " credit.");
                    }
                }
                LVoucher.SubmitVoucher(voucher.ID);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Submit, Core.Enumerations.ActionTable.Voucher, "Voucher No. " + voucher.ID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_DenyVoucher([DataSourceRequest] DataSourceRequest request, int id)
        {
            try
            {
                var voucher = LVoucher.GetVoucher(id);
                LVoucher.DenyVoucher(voucher.ID);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Deny, Core.Enumerations.ActionTable.Voucher, "Voucher No. " + voucher.ID);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("data_error", ex.Message);
            }
            return Json(new[] { id }.ToDataSourceResult(request, ModelState));
        }
    }
}

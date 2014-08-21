using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Microsoft.ReportingServices;
using Microsoft.Reporting.WebForms;
using Web.Core.DataAccess;
using Web.Core;


namespace Web.Reports
{
    public partial class ReportViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserSession.IsLoggedIn)
                Response.Redirect("~");
            
            if (!this.IsPostBack)
            {

                if (IsPostBack)
                    return;
                var type = Convert.ToInt32(WebContext.GetQueryStringValue("t"));

                switch (type)
                {
                    case 1:
                        PrintVoucher();
                        break;
                    case 2:
                        PrintInvoice();
                        break;
                    case 3:
                        ReceiveOR();
                        break;
                    case 4:
                        PrintCheck();
                        break;
                    default:
                        Response.Redirect("~");
                        break;
                }


            }
        }

        private void PrintVoucher()
        {
            var id = Convert.ToInt32(WebContext.GetQueryStringValue("id"));

            var voucher = LVoucher.GetVoucher(id);
            if (voucher != null)
            {
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.OR, Core.Enumerations.ActionTable.SalesInvoice, voucher.ID.ToString());
                var entries = LVoucherEntry.GetEntries(id).OrderBy(a => a.Credit);

                rpt.ProcessingMode = ProcessingMode.Local;
                rpt.LocalReport.ReportPath = Server.MapPath("~/Core/Reports/AccountsPayable.rdlc");
                var sig = UserSession.User.Firstname + " " + UserSession.User.Middlename.Substring(0, 1) + ". " + UserSession.User.Lastname;

                var par = new List<ReportParameter>();
                par.Add(new ReportParameter("CompanyName", Constants.CoName()));
                par.Add(new ReportParameter("CompanyAddress", Constants.CoAddress()));
                par.Add(new ReportParameter("Payee", voucher.PayeeName));
                par.Add(new ReportParameter("Particulars", voucher.Remarks));
                par.Add(new ReportParameter("Project", voucher.ProjectConsolidated));
                par.Add(new ReportParameter("CheckNo", voucher.CheckNo == null || voucher.CheckNo == "" ? "-" : voucher.CheckNo));
                par.Add(new ReportParameter("CheckDate", voucher.CheckDate != null ? ((DateTime)voucher.CheckDate).ToShortDateString() : ""));
                par.Add(new ReportParameter("Amount", ((decimal)(voucher.CheckAmount == null ? 0 : voucher.CheckAmount)).ToString("#,###.00")));
                par.Add(new ReportParameter("Debit", entries.Sum(a => a.Debit).ToString("#,###.00")));
                par.Add(new ReportParameter("Credit", entries.Sum(a => a.Credit).ToString("#,###.00")));
                par.Add(new ReportParameter("Signatory", sig));
                par.Add(new ReportParameter("CheckBy", Constants.Checker()));
                par.Add(new ReportParameter("ApprovedBy", Constants.Approver()));
                par.Add(new ReportParameter("DateCreated", voucher.DateAdded != null ? ((DateTime)voucher.DateAdded).ToShortDateString() : ""));

                rpt.LocalReport.SetParameters(par);

                rpt.LocalReport.Refresh();

                ReportDataSource dataSource = new ReportDataSource("dsVoucherEntries", entries);
                rpt.LocalReport.DataSources.Add(dataSource);

                var filename = "VOUCHER" + voucher.PayeeName.Replace(",", "").Replace(" ", "").ToUpper() + DateTime.Now.ToString("MMddyyyymmss");
                GeneratePDF(rpt, filename);
            }
        }

        private void PrintInvoice()
        {
            var id = Convert.ToInt32(WebContext.GetQueryStringValue("id"));

            var receivable = LSalesInvoice.GetSalesInvoice(id);
            if (receivable != null)
            {
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Print, Core.Enumerations.ActionTable.SalesInvoice, receivable.ID.ToString());
                var entries = LSalesInvoiceEntry.GetEntries(receivable.ID);

                rpt.ProcessingMode = ProcessingMode.Local;
                rpt.LocalReport.ReportPath = Server.MapPath("~/Core/Reports/SalesInvoice.rdlc");
                var sig = UserSession.User.Firstname + " " + UserSession.User.Middlename.Substring(0, 1) + ". " + UserSession.User.Lastname;

                var par = new List<ReportParameter>();
                par.Add(new ReportParameter("CompanyName", Constants.CoName()));
                par.Add(new ReportParameter("CompanyAddress", Constants.CoAddress()));
                par.Add(new ReportParameter("CompanyTIN", Constants.CoTIN()));
                par.Add(new ReportParameter("Customer", receivable.Customer));
                par.Add(new ReportParameter("TIN", receivable.TIN == "" ? "-" : receivable.TIN));
                par.Add(new ReportParameter("Address", receivable.Address == "" ? "-" : receivable.Address));
                par.Add(new ReportParameter("PwdIdNo", receivable.PWDNo == "" ? "-" : receivable.PWDNo));
                par.Add(new ReportParameter("PwdSignature", receivable.PWDSignature == "" ? "-" : receivable.PWDSignature));
                par.Add(new ReportParameter("BusinessName", receivable.Business == "" ? "-" : receivable.Business));
                par.Add(new ReportParameter("VATableSales", receivable.NetAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("VATExemptSales", receivable.VatExemptSales.ToString("#,###.00")));
                par.Add(new ReportParameter("ZeroRatedSales", receivable.ZeroRatedSales.ToString("#,###.00")));
                par.Add(new ReportParameter("VATAmount", receivable.VATAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("TotalSalesVATInc", receivable.TotalAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("LessVAT", receivable.VATAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("AmountNetVAT", receivable.NetAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("LessPWDDiscount", receivable.PWDDiscount.ToString("#,###.00")));
                par.Add(new ReportParameter("TotalAmountDue", receivable.TotalAmountDue.ToString("#,###.00")));
                par.Add(new ReportParameter("InvoiceNo", receivable.InvoiceNo.ToString()));
                par.Add(new ReportParameter("Signatory", sig));
                par.Add(new ReportParameter("PercentCompletion", receivable.PercentCompletion.ToString("#,###.00")));
                par.Add(new ReportParameter("AmountDue", receivable.AmountDue.ToString("#,###.00")));
                par.Add(new ReportParameter("AddVAT", receivable.VATAmount.ToString("#,###.00")));


                rpt.LocalReport.SetParameters(par);

                rpt.LocalReport.Refresh();

                ReportDataSource dataSource = new ReportDataSource("dsSalesInvoiceEntry", entries);
                rpt.LocalReport.DataSources.Add(dataSource);

                var filename = "INVOICEENTRIES" + receivable.Customer.Replace(",", "").Replace(" ", "").ToUpper() + DateTime.Now.ToString("MMddyyyymmss");
                GeneratePDF(rpt, filename);
            }
        }

        private void ReceiveOR()
        {
            var id = Convert.ToInt32(WebContext.GetQueryStringValue("id"));

            var receivable = LSalesInvoice.GetSalesInvoice(id);
            if (receivable != null)
            {
                LSalesInvoice.Collect(receivable);
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.OR, Core.Enumerations.ActionTable.SalesInvoice, receivable.ID.ToString());
                var entries = LSalesInvoiceEntry.GetEntries(receivable.ID);

                rpt.ProcessingMode = ProcessingMode.Local;
                rpt.LocalReport.ReportPath = Server.MapPath("~/Core/Reports/CollectedReceivables.rdlc");
                var sig = UserSession.User.Firstname + " " + UserSession.User.Middlename.Substring(0, 1) + ". " + UserSession.User.Lastname;

                var par = new List<ReportParameter>();
                par.Add(new ReportParameter("CompanyName", Constants.CoName()));
                par.Add(new ReportParameter("CompanyAddress", Constants.CoAddress()));
                par.Add(new ReportParameter("CompanyTIN", Constants.CoTIN()));
                par.Add(new ReportParameter("Customer", receivable.Customer));
                par.Add(new ReportParameter("TIN", receivable.TIN == "" ? "-" : receivable.TIN));
                par.Add(new ReportParameter("Address", receivable.Address == "" ? "-" : receivable.Address));
                par.Add(new ReportParameter("PwdIdNo", receivable.PWDNo == "" ? "-" : receivable.PWDNo));
                par.Add(new ReportParameter("PwdSignature", receivable.PWDSignature == "" ? "-" : receivable.PWDSignature));
                par.Add(new ReportParameter("BusinessName", receivable.Business == "" ? "-" : receivable.Business));
                par.Add(new ReportParameter("VATableSales", receivable.NetAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("VATExemptSales", receivable.VatExemptSales.ToString("#,###.00")));
                par.Add(new ReportParameter("ZeroRatedSales", receivable.ZeroRatedSales.ToString("#,###.00")));
                par.Add(new ReportParameter("VATAmount", receivable.VATAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("TotalSalesVATInc", receivable.TotalAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("LessVAT", receivable.VATAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("AmountNetVAT", receivable.NetAmount.ToString("#,###.00")));
                par.Add(new ReportParameter("LessPWDDiscount", receivable.PWDDiscount.ToString("#,###.00")));
                par.Add(new ReportParameter("TotalAmountDue", receivable.TotalAmountDue.ToString("#,###.00")));
                par.Add(new ReportParameter("InvoiceNo", receivable.ID.ToString()));
                par.Add(new ReportParameter("Signatory", sig));

                rpt.LocalReport.SetParameters(par);
                
                rpt.LocalReport.Refresh();

                ReportDataSource dataSource = new ReportDataSource("dsSalesInvoiceEntry", entries);
                rpt.LocalReport.DataSources.Add(dataSource);

                var filename = "INVOICE" + receivable.Customer.Replace(",", "").Replace(" ", "").ToUpper() + DateTime.Now.ToString("MMddyyyymmss");
                GeneratePDF(rpt, filename);
            }
        }

        private void PrintCheck()
        {
            var id = Convert.ToInt32(WebContext.GetQueryStringValue("id"));
            var check = Convert.ToInt32(WebContext.GetQueryStringValue("check"));

            var voucher = LVoucher.GetVoucher(id);
            if (voucher != null)
            {
                LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.OR, Core.Enumerations.ActionTable.SalesInvoice, voucher.ID.ToString());
                var entries = LVoucherEntry.GetEntries(voucher.ID);

                rpt.ProcessingMode = ProcessingMode.Local;
                switch (check)
                {
                    case 1: rpt.LocalReport.ReportPath = Server.MapPath("~/Core/Reports/CheckBPI.rdlc"); break;
                    case 2: rpt.LocalReport.ReportPath = Server.MapPath("~/Core/Reports/CheckMetroBank.rdlc"); break;
                    case 3: rpt.LocalReport.ReportPath = Server.MapPath("~/Core/Reports/CheckChinabank.rdlc"); break;
                    default: break;
                }
                


                var sig = UserSession.User.Firstname + " " + UserSession.User.Middlename.Substring(0, 1) + ". " + UserSession.User.Lastname;

                var par = new List<ReportParameter>();
                par.Add(new ReportParameter("Payee", "** " + voucher.PayeeName + " **"));
                par.Add(new ReportParameter("CheckDate", voucher.CheckDate != null ? ((DateTime)voucher.CheckDate).ToShortDateString() : ""));
                var checkAmount = (decimal)(voucher.CheckAmount == null ? 0 : voucher.CheckAmount);
                par.Add(new ReportParameter("Amount", "** " + checkAmount.ToString("#,###.00") + " **"));
                par.Add(new ReportParameter("AmountText","** " + Utility.NumberToCurrencyText(checkAmount) + " **"));

                rpt.LocalReport.SetParameters(par);

                rpt.LocalReport.Refresh();

                //ReportDataSource dataSource = new ReportDataSource("dsVoucherEntries", entries);
                //rpt.LocalReport.DataSources.Add(dataSource);

                var filename = "CHECK" + voucher.PayeeName.Replace(",", "").Replace(" ", "").ToUpper() + DateTime.Now.ToString("MMddyyyymmss");
                GeneratePDF(rpt, filename);
            }
        }


        private void GeneratePDF(Microsoft.Reporting.WebForms.ReportViewer rpt, string filename)
        {
            Warning[] warnings;
            string[] streamIds;
            string mimeType = string.Empty;
            string encoding = string.Empty;
            string extension = string.Empty;
            rpt.ShowParameterPrompts = true;
            rpt.ProcessingMode = ProcessingMode.Local;

            byte[] bytes = rpt.LocalReport.Render("PDF", null, out mimeType, out encoding, out extension, out streamIds, out warnings);

            Response.Buffer = true;
            Response.Clear();
            Response.ContentType = mimeType;
            Response.AddHeader("content-disposition", "attachment; filename=" + filename + "." + extension);
            Response.BinaryWrite(bytes);
            Response.Flush();
        }
    }
}
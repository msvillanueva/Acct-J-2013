using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aspose.Cells;
using Web.Core;
using Web.Core.DataAccess;

namespace Web.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/

        public ActionResult Index()
        {
            ViewBag.Title = "Print Reports";
            ViewBag.SubTitle = "Print reports within the specified date range.";

            var projects = LProject.GetProjects();
            ViewBag.ProjectLookUp = projects.ToList();
            ViewBag.Projects = projects.ToList()
                .Select(a => new SelectListItem()
                {
                    Value = a.ID.ToString(),
                    Text = a.Name
                })
                .ToList();

            return View(LReportPermission.GetAllowedReports((int)UserSession.Role));
        }

        public ActionResult CollectionsReport(string dateFrom, string dateTo)
        {
            //Open template

            string path = Server.MapPath(@"~\Content\temp\collectionsreport.xlsx");
            FileInfo file = new FileInfo(path);
            var stream = new System.IO.MemoryStream();

            Workbook workbook = WorkbookGenerator.GenerateCollectionsReport(dateFrom, dateTo);
            workbook.Save(stream, FileFormatType.Excel2007Xlsx);

            Response.ClearContent();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=CollectionsReport" + DateTime.Now.ToShortDateString().Replace("-", "") + ".xlsx");
            Response.AddHeader("ContentType", "application/vnd.ms-excel");
            stream.WriteTo(Response.OutputStream);

            if (file.Exists)
                file.Delete();

            return new EmptyResult();
        }

        public ActionResult SalesReport(string dateFrom, string dateTo)
        {
            //Open template

            string path = Server.MapPath(@"~\Content\temp\salesreport.xlsx");
            FileInfo file = new FileInfo(path);
            var stream = new System.IO.MemoryStream();

            Workbook workbook = WorkbookGenerator.GenerateSalesReport(dateFrom, dateTo);
            workbook.Save(stream, FileFormatType.Excel2007Xlsx);

            Response.ClearContent();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=SalesReport" + DateTime.Now.ToShortDateString().Replace("-", "") + ".xlsx");
            Response.AddHeader("ContentType", "application/vnd.ms-excel");
            stream.WriteTo(Response.OutputStream);

            if (file.Exists)
                file.Delete();

            return new EmptyResult();
        }

        public ActionResult ExpensesReport(int projectid, string dateFrom, string dateTo)
        {
            //Open template
            var project = LProject.GetProject(projectid);
            if (project == null)
                throw new Exception("Invalid Project ID!");

            string path = Server.MapPath(@"~\Content\temp\expensereport.xlsx");
            FileInfo file = new FileInfo(path);
            var stream = new System.IO.MemoryStream();

            Workbook workbook = WorkbookGenerator.GenerateExpenseReport(project, dateFrom, dateTo);
            workbook.Save(stream, FileFormatType.Excel2007Xlsx);

            Response.ClearContent();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=ExpenseReport" + DateTime.Now.ToShortDateString().Replace("-", "") + ".xlsx");
            Response.AddHeader("ContentType", "application/vnd.ms-excel");
            stream.WriteTo(Response.OutputStream);

            if (file.Exists)
                file.Delete();

            return new EmptyResult();
        }

        public ActionResult BalanceSheetReport(string dateFrom, string dateTo)
        {
            //Open template

            string path = Server.MapPath(@"~\Content\temp\balancesheetreport.xlsx");
            FileInfo file = new FileInfo(path);
            var stream = new System.IO.MemoryStream();

            Workbook workbook = WorkbookGenerator.GenerateBalanceSheetReport(dateFrom, dateTo);
            workbook.Save(stream, FileFormatType.Excel2007Xlsx);

            Response.ClearContent();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=BalanceSheetReport" + DateTime.Now.ToShortDateString().Replace("-", "") + ".xlsx");
            Response.AddHeader("ContentType", "application/vnd.ms-excel");
            stream.WriteTo(Response.OutputStream);

            if (file.Exists)
                file.Delete();

            return new EmptyResult();
        }

        public ActionResult TrialBalanceReport(string dateFrom, string dateTo)
        {
            //Open template

            string path = Server.MapPath(@"~\Content\temp\trialbalancereport.xlsx");
            FileInfo file = new FileInfo(path);
            var stream = new System.IO.MemoryStream();

            Workbook workbook = WorkbookGenerator.GenerateTrialBalanceReport(dateTo);
            workbook.Save(stream, FileFormatType.Excel2007Xlsx);

            Response.ClearContent();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=TrialBalanceReport" + DateTime.Now.ToShortDateString().Replace("-", "") + ".xlsx");
            Response.AddHeader("ContentType", "application/vnd.ms-excel");
            stream.WriteTo(Response.OutputStream);

            if (file.Exists)
                file.Delete();

            return new EmptyResult();
        }

        public ActionResult PostDatedCheckReport(string dateFrom, string dateTo)
        {
            //Open template

            string path = Server.MapPath(@"~\Content\temp\postdatedcheckreport.xlsx");
            FileInfo file = new FileInfo(path);
            var stream = new System.IO.MemoryStream();

            Workbook workbook = WorkbookGenerator.GeneratePostDatedCheckReport();
            workbook.Save(stream, FileFormatType.Excel2007Xlsx);

            Response.ClearContent();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=PostDatedCheckReport" + DateTime.Now.ToShortDateString().Replace("-", "") + ".xlsx");
            Response.AddHeader("ContentType", "application/vnd.ms-excel");
            stream.WriteTo(Response.OutputStream);

            if (file.Exists)
                file.Delete();

            return new EmptyResult();
        }

        public ActionResult InputTaxReport(string dateFrom, string dateTo)
        {
            //Open template

            string path = Server.MapPath(@"~\Content\temp\inputtaxreport.xlsx");
            FileInfo file = new FileInfo(path);
            var stream = new System.IO.MemoryStream();

            Workbook workbook = WorkbookGenerator.GenerateInputTaxReport(dateFrom, dateTo);
            workbook.Save(stream, FileFormatType.Excel2007Xlsx);

            Response.ClearContent();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=InputTaxReport" + DateTime.Now.ToShortDateString().Replace("-", "") + ".xlsx");
            Response.AddHeader("ContentType", "application/vnd.ms-excel");
            stream.WriteTo(Response.OutputStream);

            if (file.Exists)
                file.Delete();

            return new EmptyResult();
        }

        public ActionResult ExpandedTaxReport(string dateFrom, string dateTo)
        {
            //Open template

            string path = Server.MapPath(@"~\Content\temp\expandedreport.xlsx");
            FileInfo file = new FileInfo(path);
            var stream = new System.IO.MemoryStream();

            Workbook workbook = WorkbookGenerator.GenerateExpandedTaxReport(dateFrom, dateTo);
            workbook.Save(stream, FileFormatType.Excel2007Xlsx);

            Response.ClearContent();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=ExpandedTaxReport" + DateTime.Now.ToShortDateString().Replace("-", "") + ".xlsx");
            Response.AddHeader("ContentType", "application/vnd.ms-excel");
            stream.WriteTo(Response.OutputStream);

            if (file.Exists)
                file.Delete();

            return new EmptyResult();
        }

        public ActionResult IncomeStatementReport(string dateFrom, string dateTo)
        {
            //Open template

            string path = Server.MapPath(@"~\Content\temp\incomestatementreport.xlsx");
            FileInfo file = new FileInfo(path);
            var stream = new System.IO.MemoryStream();

            Workbook workbook = WorkbookGenerator.GenerateIncomeStatementReport(dateFrom, dateTo);
            workbook.Save(stream, FileFormatType.Excel2007Xlsx);

            Response.ClearContent();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=IncomeStatementReport" + DateTime.Now.ToShortDateString().Replace("-", "") + ".xlsx");
            Response.AddHeader("ContentType", "application/vnd.ms-excel");
            stream.WriteTo(Response.OutputStream);

            if (file.Exists)
                file.Delete();

            return new EmptyResult();
        }

    }
}

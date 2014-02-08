using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LSalesInvoice
    {
        #region Create

        public static int Create(CSalesInvoice invoice)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = new SalesInvoice()
                {
                    InvoiceNo = invoice.InvoiceNo,
                    AddedByID = invoice.AddedByID,
                    ProjectID = invoice.ProjectID,
                    DateAdded = invoice.DateAdded,
                    Customer = invoice.Customer,
                    Address = invoice.Address == null ? "" : invoice.Address,
                    TIN = invoice.TIN == null ? "" : invoice.TIN,
                    Business = invoice.Business == null ? "" : invoice.Business,
                    Terms = invoice.Terms == null ? "" : invoice.Terms,
                    PWDNo = invoice.PWDNo == null ? "" : invoice.PWDNo,
                    PWDSignature = invoice.PWDSignature == null ? "" : invoice.PWDSignature,
                    PercentCompletion = invoice.PercentCompletion,
                    IsSubmitted = false,
                    IsCollected = false,
                    PWDDiscount = 0,
                    VatExemptSales = 0,
                    ZeroRatedSales = 0,
                    Vatable = invoice.Vatable,
                    ORNo = ""
                };
                db.SalesInvoices.Add(obj);
                db.SaveChanges();
                return obj.ID;
            }
        }

        #endregion

        #region Read

        public static List<CSalesInvoice> GetSalesInvoices(bool submittedOnly = false)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.SalesInvoices
                    .Include("User")
                    .Include("Project")
                    .Include("SalesInvoiceEntry")
                    .Where(a => a.IsSubmitted || !submittedOnly)
                    .Select(a => new CSalesInvoice()
                    {
                        InvoiceNo = a.InvoiceNo,
                        ID = a.ID,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        ProjectCode = a.Project.Code,
                        Customer = a.Customer,
                        Address = a.Address,
                        TIN = a.TIN,
                        Business = a.Business,
                        DateAdded = a.DateAdded,
                        Terms = a.Terms,
                        PWDNo = a.PWDNo,
                        PWDSignature = a.PWDSignature,
                        PercentCompletion = a.PercentCompletion,
                        PWDDiscount = a.PWDDiscount,
                        VatExemptSales = a.VatExemptSales,
                        ZeroRatedSales = a.ZeroRatedSales,
                        IsSubmitted = a.IsSubmitted,
                        DateSubmitted = a.DateSubmitted,
                        IsCollected = a.IsCollected,
                        DateCollected = a.DateCollected,
                        TotalAmount = a.SalesInvoiceEntries.Count() > 0 ? a.SalesInvoiceEntries.Sum(b => b.Quantity * b.UnitPrice) : 0,
                        Vatable = a.Vatable,
                        ORNo = a.ORNo
                    })
                    .OrderByDescending(a => a.DateAdded)
                    .ToList();
            }
        }

        public static List<CSalesInvoice> GetSalesInvoices(DateTime dateFrom, DateTime dateTo, bool submittedOnly = false)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.SalesInvoices
                    .Include("User")
                    .Include("Project")
                    .Include("SalesInvoiceEntry")
                    .Where(a => (a.IsSubmitted || !submittedOnly) && a.DateAdded >= dateFrom && a.DateAdded <= dateTo)
                    .Select(a => new CSalesInvoice()
                    {
                        InvoiceNo = a.InvoiceNo,
                        ID = a.ID,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        ProjectCode = a.Project.Code,
                        Customer = a.Customer,
                        Address = a.Address,
                        TIN = a.TIN,
                        Business = a.Business,
                        DateAdded = a.DateAdded,
                        Terms = a.Terms,
                        PWDNo = a.PWDNo,
                        PWDSignature = a.PWDSignature,
                        PercentCompletion = a.PercentCompletion,
                        PWDDiscount = a.PWDDiscount,
                        VatExemptSales = a.VatExemptSales,
                        ZeroRatedSales = a.ZeroRatedSales,
                        IsSubmitted = a.IsSubmitted,
                        DateSubmitted = a.DateSubmitted,
                        IsCollected = a.IsCollected,
                        DateCollected = a.DateCollected,
                        TotalAmount = a.SalesInvoiceEntries.Count() > 0 ? a.SalesInvoiceEntries.Sum(b => b.Quantity * b.UnitPrice) : 0,
                        Vatable = a.Vatable,
                        ORNo = a.ORNo
                    })
                    .OrderByDescending(a => a.DateAdded)
                    .ToList();
            }
        }

        public static CSalesInvoice GetSalesInvoice(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var invoice = db.SalesInvoices
                    .Include("User")
                    .Include("Project")
                    .Include("SalesInvoiceEntry")
                    .Where(a => a.ID == id)
                    .Select(a => new CSalesInvoice()
                    {
                        InvoiceNo = a.InvoiceNo,
                        ID = a.ID,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project == null ? "" : a.Project.Name,
                        ProjectCode = a.Project == null ? "" : a.Project.Code,
                        Customer = a.Customer,
                        Address = a.Address,
                        TIN = a.TIN,
                        Business = a.Business,
                        DateAdded = a.DateAdded,
                        Terms = a.Terms,
                        PWDNo = a.PWDNo,
                        PWDSignature = a.PWDSignature,
                        PercentCompletion = a.PercentCompletion,
                        PWDDiscount = a.PWDDiscount,
                        VatExemptSales = a.VatExemptSales,
                        ZeroRatedSales = a.ZeroRatedSales,
                        IsSubmitted = a.IsSubmitted,
                        DateSubmitted = a.DateSubmitted,
                        IsCollected = a.IsCollected,
                        DateCollected = a.DateCollected,
                        TotalAmount = a.SalesInvoiceEntries.Count() > 0 ? a.SalesInvoiceEntries.Sum(b => b.Quantity * b.UnitPrice) : 0,
                        Vatable = a.Vatable,
                        ORNo = a.ORNo
                    })
                    .FirstOrDefault();

                if (invoice != null)
                {
                    var entryProjects = db.SalesInvoiceEntries
                        .Include("Project")
                        .Where(a => a.SalesInvoiceID == invoice.ID)
                        .Select(a => a.Project.Name)
                        .Distinct()
                        .ToList();

                    if (entryProjects.Count() > 0)
                    {
                        var projectString = String.Join(", ", entryProjects.ToArray());
                        var projectCons = projectString;
                        invoice.ProjectConsolidated = projectCons;
                    }
                }
                return invoice;
            }
        }

        public static int GetPercentCompletion(int projectID)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.SalesInvoices
                    .Where(a => a.ProjectID == projectID)
                    .OrderByDescending(a => a.PercentCompletion)
                    .FirstOrDefault();
                if (obj != null)
                    return obj.PercentCompletion;
                return 0;
            }
        }



        #endregion

        #region Update

        public static void Update(CSalesInvoice invoice)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.SalesInvoices.FirstOrDefault(a => a.ID == invoice.ID);
                obj.InvoiceNo = invoice.InvoiceNo;
                obj.ProjectID = invoice.ProjectID;
                obj.Customer = invoice.Customer;
                obj.Address = invoice.Address == null ? "" : invoice.Address;
                obj.TIN = invoice.TIN == null ? "" : invoice.TIN;
                obj.Business = invoice.Business == null ? "" : invoice.Business;
                obj.Terms = invoice.Terms == null ? "" : invoice.Terms;
                obj.PWDNo = invoice.PWDNo == null ? "" : invoice.PWDNo;
                obj.PWDSignature = invoice.PWDSignature == null ? "" : invoice.PWDSignature;
                obj.PercentCompletion = invoice.PercentCompletion;
                obj.Vatable = invoice.Vatable;
                obj.DateAdded = invoice.DateAdded;
                obj.ORNo = invoice.ORNo;
                db.SaveChanges();
            }
        }

        public static void Submit(CSalesInvoice invoice)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.SalesInvoices.FirstOrDefault(a => a.ID == invoice.ID);
                obj.IsSubmitted = true;
                obj.DateSubmitted = DateTime.Now;
                obj.PWDDiscount = invoice.PWDDiscount;
                obj.VatExemptSales = invoice.VatExemptSales;
                obj.ZeroRatedSales = invoice.ZeroRatedSales;
                obj.ORNo = invoice.ORNo;
                obj.IsCollected = true;
                obj.DateCollected = DateTime.Now;
                db.SaveChanges();
            }
        }

        public static void Collect(CSalesInvoice invoice)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.SalesInvoices.FirstOrDefault(a => a.ID == invoice.ID);
                if (obj.IsCollected)
                    return;
                obj.IsCollected = true;
                obj.DateCollected = DateTime.Now;
                db.SaveChanges();
            }
        }

        public static void Deny(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.SalesInvoices.FirstOrDefault(a => a.ID == id);
                obj.IsSubmitted = false;
                obj.IsCollected = false;
                obj.DateCollected = null;
                obj.DateSubmitted = null;
                obj.PWDDiscount = 0;
                obj.VatExemptSales = 0;
                obj.ZeroRatedSales = 0;
                db.SaveChanges();
            }
        }

        #endregion

        #region Delete

        public static void Delete(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.SalesInvoices.FirstOrDefault(a => a.ID == id);
                if (db.SalesInvoiceEntries.Where(a => a.SalesInvoiceID == obj.ID).Count() > 0)
                    throw new Exception("Cannot delete invoice with entries.");
                db.SalesInvoices.Remove(obj);
                db.SaveChanges();
            }
        }

        #endregion
    }
}
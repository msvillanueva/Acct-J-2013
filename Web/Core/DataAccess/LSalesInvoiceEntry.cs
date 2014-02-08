using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LSalesInvoiceEntry
    {
        #region Create

        public static int Create(CSalesInvoiceEntry entry)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = new SalesInvoiceEntry()
                {
                    SalesInvoiceID = entry.SalesInvoiceID,
                    Quantity = entry.Quantity,
                    Unit = entry.Unit,
                    Articles = entry.Articles,
                    UnitPrice = entry.UnitPrice,
                    AddedbyID = entry.AddedByID,
                    DateAdded = DateTime.Now,
                    ProjectID = entry.ProjectID,
                    AccountEntryID = entry.AccountEntryID
                };
                db.SalesInvoiceEntries.Add(obj);
                db.SaveChanges();
                return obj.ID;
            }
        }

        #endregion

        #region Read

        public static List<CSalesInvoiceEntry> GetEntries(int invoiceID)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.SalesInvoiceEntries
                    .Include("SalesInvoice")
                    .Include("User")
                    .Include("AccounEntry")
                    .Where(a => a.SalesInvoiceID == invoiceID)
                    .Select(a => new CSalesInvoiceEntry()
                    {
                        ID = a.ID,
                        SalesInvoiceID = a.SalesInvoiceID,
                        Quantity = a.Quantity,
                        Unit = a.Unit,
                        Articles = a.Articles,
                        UnitPrice = a.UnitPrice,
                        AddedByID = a.AddedbyID,
                        DateAdded = a.DateAdded,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        ProjectCode = a.Project.Code,
                        AccountEntryID = a.AccountEntryID,
                        AccountEntryCode = a.AccountEntry != null ? a.AccountEntry.Code : "",
                        AccountEntryName = a.AccountEntry != null ? a.AccountEntry.Title : ""
                    })
                    .OrderBy(a => a.Articles)
                    .ToList();
            }
        }

        public static List<CSalesInvoiceEntry> GetEntries(DateTime dateFrom, DateTime dateTo)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.SalesInvoiceEntries
                    .Include("SalesInvoice")
                    .Include("User")
                    .Include("AccounEntry")
                    .Where(a => a.DateAdded >= dateFrom && a.DateAdded <= dateTo)
                    .Select(a => new CSalesInvoiceEntry()
                    {
                        ID = a.ID,
                        SalesInvoiceID = a.SalesInvoiceID,
                        Quantity = a.Quantity,
                        Unit = a.Unit,
                        Articles = a.Articles,
                        UnitPrice = a.UnitPrice,
                        AddedByID = a.AddedbyID,
                        DateAdded = a.DateAdded,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        ProjectCode = a.Project.Code,
                        AccountEntryID = a.AccountEntryID,
                        AccountEntryCode = a.AccountEntry != null ? a.AccountEntry.Code : "",
                        AccountEntryName = a.AccountEntry != null ? a.AccountEntry.Title : ""
                    })
                    .OrderBy(a => a.Articles)
                    .ToList();
            }
        }

        public static CSalesInvoiceEntry GetEntry(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.SalesInvoiceEntries
                    .Include("SalesInvoice")
                    .Include("User")
                    .Include("AccounEntry")
                    .Where(a => a.ID == id)
                    .Select(a => new CSalesInvoiceEntry()
                    {
                        ID = a.ID,
                        SalesInvoiceID = a.SalesInvoiceID,
                        Quantity = a.Quantity,
                        Unit = a.Unit,
                        Articles = a.Articles,
                        UnitPrice = a.UnitPrice,
                        AddedByID = a.AddedbyID,
                        DateAdded = a.DateAdded,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        ProjectCode = a.Project.Code,
                        AccountEntryID = a.AccountEntryID,
                        AccountEntryCode = a.AccountEntry != null ? a.AccountEntry.Code : "",
                        AccountEntryName = a.AccountEntry != null ? a.AccountEntry.Title : ""
                    })
                    .FirstOrDefault();
            }
        }

        #endregion

        #region Update

        public static void Update(CSalesInvoiceEntry entry)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.SalesInvoiceEntries.FirstOrDefault(a => a.ID == entry.ID);
                obj.Quantity = entry.Quantity;
                obj.Unit = entry.Unit;
                obj.Articles = entry.Articles;
                obj.UnitPrice = entry.UnitPrice;
                obj.ProjectID = entry.ProjectID;
                obj.AccountEntryID = entry.AccountEntryID;
                db.SaveChanges();
            }
        }

        #endregion

        #region Delete

        public static void Delete(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.SalesInvoiceEntries.FirstOrDefault(a => a.ID == id);
                db.SalesInvoiceEntries.Remove(obj);
                db.SaveChanges();
            }
        }

        #endregion
    }
}
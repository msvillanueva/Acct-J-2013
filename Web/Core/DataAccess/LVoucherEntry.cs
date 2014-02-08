using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LVoucherEntry
    {
        #region Create

        public static int Create(CVoucherEntry entry)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = new VoucherEntry()
                {
                    VoucherID = entry.VoucherID,
                    AccountEntryID = entry.AccountEntryID,
                    AddedByID = entry.AddedByID,
                    BankID = entry.BankID,
                    DateAdded = DateTime.Now,
                    Debit = entry.Debit,
                    Credit = entry.Credit,
                    ProjectID = entry.ProjectID
                };
                db.VoucherEntries.Add(obj);
                db.SaveChanges();
                return obj.ID;
            }
        }

        #endregion

        #region Read

        public static List<CVoucherEntry> GetEntries(int voucherID)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.VoucherEntries
                    .Include("AccountEntry")
                    .Include("Voucher")
                    .Include("Bank")
                    .Include("User")
                    .Include("Project")
                    .Include("Payee")
                    .Where(a => a.VoucherID == voucherID)
                    .Select(a => new CVoucherEntry()
                    {
                        ID = a.ID,
                        AccountEntryCode = a.AccountEntry.Code,
                        AccountEntryID = a.AccountEntryID,
                        AccountEntryName = a.AccountEntry.Title,
                        AccountEntryTypeID = a.AccountEntry.AccountEntryTypeID,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        BankID = a.BankID,
                        BankName = a.Bank.Name,
                        BankCode = a.Bank.Code,
                        Debit = a.Debit,
                        Credit = a.Credit,
                        VoucherID = a.VoucherID,
                        DateAdded = a.DateAdded,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        ProjectCode = a.Project.Code,
                        CheckNo = a.Voucher.CheckNo,
                        CheckDate = a.Voucher.CheckDate,
                        PayeeName = a.Voucher.Payee.Name
                    })
                    .OrderBy(a => a.AccountEntryName)
                    .ToList();
            }
        }

        public static List<CVoucherEntry> GetRptEntriesPerProject(int projectID, DateTime dateFrom, DateTime dateTo)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.VoucherEntries
                    .Include("AccountEntry")
                    .Include("Voucher")
                    .Include("Bank")
                    .Include("User")
                    .Include("Project")
                    .Include("Payee")
                    .Where(a => a.ProjectID == projectID && a.Voucher.Submitted &&
                        a.Voucher.DateSubmitted >= dateFrom && a.Voucher.DateSubmitted <= dateTo)
                    .Select(a => new CVoucherEntry()
                    {
                        ID = a.ID,
                        AccountEntryCode = a.AccountEntry.Code,
                        AccountEntryID = a.AccountEntryID,
                        AccountEntryName = a.AccountEntry.Title,
                        AccountEntryTypeID = a.AccountEntry.AccountEntryTypeID,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        BankID = a.BankID,
                        BankName = a.Bank.Name,
                        BankCode = a.Bank.Code,
                        Debit = a.Debit,
                        Credit = a.Credit,
                        VoucherID = a.VoucherID,
                        DateAdded = a.DateAdded,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        ProjectCode = a.Project.Code,
                        CheckNo = a.Voucher.CheckNo,
                        CheckDate = a.Voucher.CheckDate,
                        PayeeName = a.Voucher.Payee.Name
                    })
                    .OrderBy(a => a.AccountEntryName)
                    .ToList();
            }
        }

        public static List<CVoucherEntry> GetEntries(DateTime dateFrom, DateTime dateTo)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.VoucherEntries
                    .Include("AccountEntry")
                    .Include("Voucher")
                    .Include("Bank")
                    .Include("User")
                    .Include("Project")
                    .Include("Payee")
                    .Where(a => a.Voucher.Submitted && a.Voucher.DateSubmitted >= dateFrom && a.Voucher.DateSubmitted <= dateTo)
                    .Select(a => new CVoucherEntry()
                    {
                        ID = a.ID,
                        AccountEntryCode = a.AccountEntry.Code,
                        AccountEntryID = a.AccountEntryID,
                        AccountEntryName = a.AccountEntry.Title,
                        AccountEntryTypeID = a.AccountEntry.AccountEntryTypeID,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        BankID = a.BankID,
                        BankName = a.Bank.Name,
                        BankCode = a.Bank.Code,
                        Debit = a.Debit,
                        Credit = a.Credit,
                        VoucherID = a.VoucherID,
                        DateAdded = a.DateAdded,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        ProjectCode = a.Project.Code,
                        CheckNo = a.Voucher.CheckNo,
                        CheckDate = a.Voucher.CheckDate,
                        PayeeName = a.Voucher.Payee.Name
                    })
                    .OrderBy(a => a.AccountEntryName)
                    .ToList();
            }
        }

        public static CVoucherEntry GetEntry(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.VoucherEntries
                    .Include("AccountEntry")
                    .Include("Voucher")
                    .Include("Bank")
                    .Include("User")
                    .Include("Payee")
                    .Where(a => a.ID == id)
                    .Select(a => new CVoucherEntry()
                    {
                        ID = a.ID,
                        AccountEntryCode = a.AccountEntry.Code,
                        AccountEntryID = a.AccountEntryID,
                        AccountEntryName = a.AccountEntry.Title,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        BankID = a.BankID,
                        BankName = a.Bank.Name,
                        BankCode = a.Bank.Code,
                        Debit = a.Debit,
                        Credit = a.Credit,
                        VoucherID = a.VoucherID,
                        DateAdded = a.DateAdded,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        ProjectCode = a.Project.Code,
                        CheckNo = a.Voucher.CheckNo,
                        CheckDate = a.Voucher.CheckDate,
                        PayeeName = a.Voucher.Payee.Name
                    })
                    .OrderBy(a => a.AccountEntryName)
                    .FirstOrDefault();
            }
        }

        #endregion

        #region Update

        public static void Update(CVoucherEntry entry)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.VoucherEntries.FirstOrDefault(a => a.ID == entry.ID);
                obj.AccountEntryID = entry.AccountEntryID;
                obj.BankID = entry.BankID;
                obj.Debit = entry.Debit;
                obj.Credit = entry.Credit;
                obj.AddedByID = entry.AddedByID;
                obj.ProjectID = entry.ProjectID;
                db.SaveChanges();
            }
        }

        #endregion

        #region Delete

        public static void Delete(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.VoucherEntries.FirstOrDefault(a => a.ID == id);
                db.VoucherEntries.Remove(obj);
                db.SaveChanges();
            }
        }

        #endregion
    }
}
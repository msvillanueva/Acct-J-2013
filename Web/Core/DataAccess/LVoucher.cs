using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LVoucher
    {
        #region Create

        public static int Create(CVoucher voucher)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = new Voucher()
                {
                    PayeeID = voucher.PayeeID,
                    ProjectID = voucher.ProjectID,
                    Remarks = voucher.Remarks,
                    CheckNo = voucher.CheckNo,
                    CheckAmount = voucher.CheckAmount,
                    CheckDate = voucher.CheckDate,
                    AddedByID = voucher.AddedByID,
                    DateAdded = voucher.DateAdded,
                    Submitted = false
                };
                db.Vouchers.Add(obj);
                db.SaveChanges();
                return obj.ID;
            }
        }

        #endregion

        #region Read

        public static List<CVoucher> GetVouchers(bool submittedOnly = false)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Vouchers
                    .Include("Project")
                    .Include("Payee")
                    .Include("User")
                    .Where(a => a.Submitted || !submittedOnly)
                    .Select(a => new CVoucher()
                    {
                        ID = a.ID,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        PayeeID = a.PayeeID,
                        PayeeName = a.Payee.Name,
                        Remarks = a.Remarks,
                        CheckNo = a.CheckNo,
                        CheckAmount = a.CheckAmount,
                        CheckDate = a.CheckDate,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        DateAdded = a.DateAdded,
                        Submitted = a.Submitted,
                        DateSubmitted = a.DateSubmitted,
                        ProjectCode = a.Project.Code
                    })
                    .OrderByDescending(a => a.DateAdded)
                    .ToList();
            }
        }

        public static List<CVoucher> GetVouchers(DateTime dateFrom, DateTime dateTo, bool submittedOnly = false)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Vouchers
                    .Include("Project")
                    .Include("Payee")
                    .Include("User")
                    .Where(a => (a.Submitted || !submittedOnly) && a.DateAdded >= dateFrom && a.DateAdded <= dateTo)
                    .Select(a => new CVoucher()
                    {
                        ID = a.ID,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        PayeeID = a.PayeeID,
                        PayeeName = a.Payee.Name,
                        Remarks = a.Remarks,
                        CheckNo = a.CheckNo,
                        CheckAmount = a.CheckAmount,
                        CheckDate = a.CheckDate,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        DateAdded = a.DateAdded,
                        Submitted = a.Submitted,
                        DateSubmitted = a.DateSubmitted,
                        ProjectCode = a.Project.Code
                    })
                    .OrderByDescending(a => a.DateAdded)
                    .ToList();
            }
        }

        public static List<CVoucher> GetPostDatedChecks()
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Vouchers
                    .Include("Project")
                    .Include("Payee")
                    .Include("User")
                    .Where(a => a.CheckDate > DateTime.Now)
                    .Select(a => new CVoucher()
                    {
                        ID = a.ID,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        PayeeID = a.PayeeID,
                        PayeeName = a.Payee.Name,
                        Remarks = a.Remarks,
                        CheckNo = a.CheckNo,
                        CheckAmount = a.CheckAmount,
                        CheckDate = a.CheckDate,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        DateAdded = a.DateAdded,
                        Submitted = a.Submitted,
                        DateSubmitted = a.DateSubmitted,
                        ProjectCode = a.Project.Code
                    })
                    .OrderByDescending(a => a.DateAdded)
                    .ToList();
            }
        }

        public static CVoucher GetVoucher(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var voucher = db.Vouchers
                    .Include("Project")
                    .Include("User")
                    .Where(a => a.ID == id)
                    .Select(a => new CVoucher()
                    {
                        ID = a.ID,
                        ProjectID = a.ProjectID,
                        ProjectName = a.Project.Name,
                        PayeeID = a.PayeeID,
                        PayeeName = a.Payee.Name,
                        Remarks = a.Remarks,
                        CheckNo = a.CheckNo,
                        CheckAmount = a.CheckAmount,
                        CheckDate = a.CheckDate == null ? DateTime.Now : a.CheckDate,
                        AddedByID = a.AddedByID,
                        AddedByName = a.User.Firstname + " " + a.User.Lastname,
                        DateAdded = a.DateAdded,
                        Submitted = a.Submitted,
                        DateSubmitted = a.DateSubmitted,
                        ProjectCode = a.Project.Code,
                        ProjectConsolidated = ""
                    })
                    .FirstOrDefault();

                if (voucher != null)
                {
                    var entryProjects = db.VoucherEntries
                        .Include("Project")
                        .Where(a => a.VoucherID == voucher.ID)
                        .Select(a => a.Project.Name)
                        .Distinct()
                        .ToList();
                    if (entryProjects.Count() > 0)
                    {
                        var projectString = String.Join(", ", entryProjects.ToArray());
                        var projectCons = projectString;
                        voucher.ProjectConsolidated = projectCons;
                    }
                }

                return voucher;
            }
        }

        #endregion

        #region Update

        public static void Update(CVoucher voucher)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Vouchers.FirstOrDefault(a => a.ID == voucher.ID);
                obj.PayeeID = voucher.PayeeID;
                obj.ProjectID = voucher.ProjectID;
                obj.Remarks = voucher.Remarks;
                obj.CheckNo = voucher.CheckNo;
                obj.CheckDate = voucher.CheckDate;
                obj.DateAdded = voucher.DateAdded;
                obj.CheckAmount = voucher.CheckAmount;
                db.SaveChanges();
            }
        }

        public static void SubmitVoucher(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Vouchers.FirstOrDefault(a => a.ID == id);
                obj.Submitted = true;
                obj.DateSubmitted = DateTime.Now;
                db.SaveChanges();
            }
        }

        public static void DenyVoucher(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Vouchers.FirstOrDefault(a => a.ID == id);
                obj.Submitted = false;
                obj.DateSubmitted = null;
                db.SaveChanges();
            }
        }

        #endregion

        #region Delete

        public static void Delete(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Vouchers.FirstOrDefault(a => a.ID == id);
                if (db.VoucherEntries.Where(a => a.VoucherID == obj.ID).Count() > 0)
                    throw new Exception("Cannot delete vouchers with entries");
                db.Vouchers.Remove(obj);
                db.SaveChanges();
            }
        }

        #endregion

    }
}
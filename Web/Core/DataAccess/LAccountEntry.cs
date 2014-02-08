using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LAccountEntry
    {
        #region Create

        public static void Create(string code, string title, Enumerations.EntryType type, int accountEntryTypeID)
        {
            using (var db=Connection.GetEntityContext())
            {
                if (db.AccountEntries.FirstOrDefault(a => a.Code == code.Trim().ToUpper()) != null)
                    throw new Exception("Account entry code is already registered.");
                if (db.AccountEntries.FirstOrDefault(a => a.Title == title.Trim()) != null)
                    throw new Exception("Account entry title is already registered.");
                var obj = new AccountEntry()
                {
                    Code = code.Trim().ToUpper(),
                    Title = title.Trim(),
                    Type = (int)type,
                    IsActive = true,
                    AccountEntryTypeID = (int)accountEntryTypeID
                };
                db.AccountEntries.Add(obj);
                db.SaveChanges();
            }
        }

        #endregion

        #region Read

        public static List<CAccountEntry> GetEntries()
        {
            using (var db = Connection.GetEntityContext())
            {
                var data =  db.AccountEntries
                    .Include("AccountEntryType")
                    .Select(a => new CAccountEntry()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Title = a.Title,
                        Type = a.Type,
                        TypeName = "",
                        IsActive = a.IsActive,
                        ClassName = a.AccountEntryType != null ? a.AccountEntryType.Type : "",
                        AccountEntryTypeID = a.AccountEntryTypeID
                    })
                    .OrderBy(a => a.Title)
                    .ToList();
                foreach (var d in data)
                {
                    d.TypeName = ((Enumerations.EntryType)d.Type).ToString();
                    d.AcctEntryType = db.AccountEntryTypes
                        .Where(a => a.ID == d.AccountEntryTypeID)
                        .Select(a => new CAccountEntryType()
                        {
                            ID = a.ID,
                            Code = a.Code,
                            ParentID = a.ParentID,
                            Type = a.Type
                        })
                        .FirstOrDefault();
                }
                return data;
            }
        }

        public static CAccountEntry GetEntry(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.AccountEntries
                    .Include("AccountEntryType")
                    .Where(a => a.ID == id)
                    .Select(a => new CAccountEntry()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Title = a.Title,
                        Type = a.Type,
                        TypeName = "",
                        IsActive = a.IsActive,
                        ClassName = a.AccountEntryType != null ? a.AccountEntryType.Type : "",
                        AccountEntryTypeID = a.AccountEntryTypeID
                    })
                    .FirstOrDefault();
                obj.TypeName = ((Enumerations.EntryType)obj.Type).ToString();
                obj.ClassName = ((Enumerations.EntryClass)obj.AccountEntryTypeID).ToString();
                obj.AcctEntryType = db.AccountEntryTypes
                        .Where(a => a.ID == obj.AccountEntryTypeID)
                        .Select(a => new CAccountEntryType()
                        {
                            ID = a.ID,
                            Code = a.Code,
                            ParentID = a.ParentID,
                            Type = a.Type
                        })
                        .FirstOrDefault();
                return obj;
            }
        }

        public static CAccountEntry GetEntry(string code)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.AccountEntries
                    .Include("AccountEntryType")
                    .Where(a => a.Code == code)
                    .Select(a => new CAccountEntry()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Title = a.Title,
                        Type = a.Type,
                        TypeName = "",
                        IsActive = a.IsActive,
                        AccountEntryTypeID = a.AccountEntryTypeID,
                        ClassName = a.AccountEntryType != null ? a.AccountEntryType.Type : ""
                    })
                    .FirstOrDefault();
                obj.TypeName = ((Enumerations.EntryType)obj.Type).ToString();
                obj.AcctEntryType = db.AccountEntryTypes
                        .Where(a => a.ID == obj.AccountEntryTypeID)
                        .Select(a => new CAccountEntryType()
                        {
                            ID = a.ID,
                            Code = a.Code,
                            ParentID = a.ParentID,
                            Type = a.Type
                        })
                        .FirstOrDefault();
                return obj;
            }
        }

        #endregion

        public static void Update(CAccountEntry entry)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.AccountEntries.FirstOrDefault(a => a.ID == entry.ID);
                if (db.AccountEntries.FirstOrDefault(a => a.Code == entry.Code.Trim().ToUpper() && a.ID != entry.ID) != null)
                    throw new Exception("Account entry code is already registered.");
                if (db.AccountEntries.FirstOrDefault(a => a.Title == entry.Title.Trim() && a.ID != entry.ID) != null)
                    throw new Exception("Account entry title is already registered.");
                
                obj.Code = entry.Code.Trim().ToUpper();
                obj.Title = entry.Title.Trim();
                obj.Type = entry.Type;
                obj.AccountEntryTypeID = entry.AccountEntryTypeID;
                db.SaveChanges();
            }
        }

        #region Delete

        public static void Delete(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.AccountEntries.FirstOrDefault(a => a.ID == id);
                // should check if bank is used in Payables
                db.AccountEntries.Remove(obj);
                db.SaveChanges();
            }
        }

        #endregion

    }
}
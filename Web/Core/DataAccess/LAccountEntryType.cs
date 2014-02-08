using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LAccountEntryType
    {
        #region Create

        public static void Create(CAccountEntryType type)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = new AccountEntryType()
                {
                    Code = type.Code,
                    Type = type.Type,
                    ParentID = type.ParentID
                };
                db.AccountEntryTypes.Add(obj);
                db.SaveChanges();
            }
        }

        #endregion

        #region Read

        public static List<CAccountEntryType> GetTypes()
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.AccountEntryTypes
                    .Include("AccountEntryType")
                    .Select(a => new CAccountEntryType()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Type = a.Type,
                        ParentID = a.ParentID,
                        Parent = a.AccountEntryType2
                    })
                    .OrderBy(a => a.Type)
                    .ToList();
            }
        }

        public static CAccountEntryType GetTypeByID(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.AccountEntryTypes
                    .Include("AccountEntryType")
                    .Where(a => a.ID == id)
                    .Select(a => new CAccountEntryType()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Type = a.Type,
                        ParentID = a.ParentID,
                        Parent = a.AccountEntryType2
                    })
                    .FirstOrDefault();
            }
        }

        public static CAccountEntryType GetTypeByString(string type)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.AccountEntryTypes
                    .Include("AccountEntryType")
                    .Where(a => a.Type.ToLower().Trim() == type.ToLower().Trim())
                    .Select(a => new CAccountEntryType()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Type = a.Type,
                        ParentID = a.ParentID,
                        Parent = a.AccountEntryType2
                    })
                    .FirstOrDefault();
            }
        }

        public static CAccountEntryType GetTypeByCode(string code)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.AccountEntryTypes
                    .Include("AccountEntryType")
                    .Where(a => a.Code.ToLower().Trim() == code.ToLower().Trim())
                    .Select(a => new CAccountEntryType()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Type = a.Type,
                        ParentID = a.ParentID,
                        Parent = a.AccountEntryType2
                    })
                    .FirstOrDefault();
            }
        }

        #endregion

        #region Update

        public static void Update(CAccountEntryType type)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.AccountEntryTypes.FirstOrDefault(a => a.ID == type.ID);
                if (obj != null)
                {
                    obj.Code = type.Code;
                    obj.Type = type.Type;
                    obj.ParentID = type.ParentID;
                    db.SaveChanges();
                }
            }
        }


        #endregion

        #region Delete

        public static void Delete(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.AccountEntryTypes.FirstOrDefault(a => a.ID == id);
                if (obj != null)
                {
                    if (db.AccountEntryTypes.Where(a => a.ParentID == obj.ID).Count() > 0)
                        throw new Exception("Cannot delete account types with children.");
                    db.AccountEntryTypes.Remove(obj);
                    db.SaveChanges();
                }
            }
        }

        #endregion

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LBank
    {
        #region Create

        public static void Create(string code, string name)
        {
            using (var db = Connection.GetEntityContext())
            {
                if (db.Banks.FirstOrDefault(a => a.Code == code.Trim().ToUpper()) != null)
                    throw new Exception("Bank code is already used by other bank.");
                if (db.Banks.FirstOrDefault(a => a.Name == name.Trim()) != null)
                    throw new Exception("Bank name is already used by other bank.");
                var obj = new Bank()
                {
                    Code = code.Trim().ToUpper(),
                    Name = name.Trim(),
                    IsActive = true
                };
                db.Banks.Add(obj);
                db.SaveChanges();
            }
        }

        #endregion

        #region Read

        public static List<CBank> GetBanks()
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Banks
                    .Where(a => a.IsActive)
                    .Select(a => new CBank()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        IsActive = a.IsActive
                    })
                    .OrderBy(a => a.Name)
                    .ToList();
            }
        }

        public static CBank GetBank(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Banks
                    .Where(a => a.IsActive && a.ID == id)
                    .Select(a => new CBank()
                    {
                        ID = a.ID,
                        Code = a.Code,
                        Name = a.Name,
                        IsActive = a.IsActive
                    })
                    .FirstOrDefault();
            }
        }

        public static bool IsBankCodeUnique(string code)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Banks.Where(a => a.Code.Trim().ToLower() == code.Trim().ToLower()).Count() == 0;
            }
        }

        public static bool IsBankNameUnique(string name)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Banks.Where(a => a.Name.Trim().ToLower() == name.Trim().ToLower()).Count() == 0;
            }
        }

        #endregion

        #region Update

        public static void Update(int id, string code, string name)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Banks.FirstOrDefault(a => a.ID == id);
                if (obj != null)
                {
                    if (db.Banks.FirstOrDefault(a => a.Code == code.Trim().ToUpper() && a.ID != id) != null)
                        throw new Exception("Bank code is already used by other bank.");
                    if (db.Banks.FirstOrDefault(a => a.Name == name.Trim() && a.ID != id) != null)
                        throw new Exception("Bank name is already used by other bank.");
                    obj.Code = code.ToUpper().Trim();
                    obj.Name = name.Trim();
                    db.SaveChanges();
                };
            }
        }

        #endregion

        #region Delete

        public static void Delete(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Banks.FirstOrDefault(a => a.ID == id);
                // should check if bank is used in Payables
                db.Banks.Remove(obj);
                db.SaveChanges();
            }
        }

        #endregion
    }
}
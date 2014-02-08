using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LPayee
    {
        #region Create

        public static void Create(CPayee payee)
        {
            using (var db = Connection.GetEntityContext())
            {
                if (db.Payees.FirstOrDefault(a => a.Name.Trim().ToLower() == payee.Name.ToLower().Trim()) != null)
                    throw new Exception("Name already exists");
                var obj = new Payee()
                {
                    Name = payee.Name.Trim(),
                    Address = payee.Address.Trim(),
                    TIN = payee.Tin.Trim(),
                    DateAdded = DateTime.Now,
                    IsActive = true
                };
                db.Payees.Add(obj);
                db.SaveChanges();
            }
        }

        #endregion

        #region Read

        public static List<CPayee> GeyPayees()
        {
            using (var db=Connection.GetEntityContext())
            {
                return db.Payees
                    .Where(a => a.IsActive == true)
                    .Select(a => new CPayee()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Address = a.Address,
                        Tin = a.TIN,
                        DateAdded = a.DateAdded,
                        IsActive = a.IsActive
                    })
                    .OrderBy(a => a.Name)
                    .ToList();
            }
        }

        public static CPayee GeyPayee(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Payees
                    .Where(a => a.ID == id)
                    .Select(a => new CPayee()
                    {
                        ID = a.ID,
                        Name = a.Name,
                        Address = a.Address,
                        Tin = a.TIN,
                        DateAdded = a.DateAdded,
                        IsActive = a.IsActive
                    })
                    .FirstOrDefault();
            }
        }

        #endregion

        #region Update

        public static void Update(CPayee payee)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Payees.FirstOrDefault(a => a.ID == payee.ID);

                if (db.Payees.Where(a => a.ID != payee.ID && a.Name.Trim().ToLower() == payee.Name.ToLower().Trim()).Count() > 0)
                    throw new Exception("Payee name already exists.");

                obj.Name = payee.Name.Trim();
                obj.Address = payee.Address.Trim();
                obj.TIN = payee.Tin.Trim();
                db.SaveChanges();
            }
        }

        #endregion

        #region Delete

        public static void Delete(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                //should check first if payee is in payables
                var obj = db.Payees.FirstOrDefault(a => a.ID == id);
                db.Payees.Remove(obj);
                db.SaveChanges();
            }
        }

        #endregion
    }
}
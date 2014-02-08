using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LUser
    {
        #region Create

        public static void Create(CUser user)
        {
            using (var db = Connection.GetEntityContext())
            {
                if (db.Users.Where(a => a.Username.ToLower().Trim() == user.Username.ToLower().Trim()).Count() > 0)
                    throw new Exception("Username is currently in use.");
                if (db.Users.Where(a => a.Email.ToLower().Trim() == user.Email.ToLower().Trim()).Count() > 0)
                    throw new Exception("Email is currently in use.");
                var o = new User()
                {
                    Username = user.Username.Trim(),
                    Password = Encryption.Encrypt(user.Password),
                    Firstname = user.Firstname,
                    MiddleName = user.Middlename,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    Role = user.Role,
                    DateAdded = DateTime.Now,
                    IsActive = user.IsActive
                };
                db.Users.Add(o);
                db.SaveChanges();
            }
        }

        #endregion

        #region Read

        public static List<CUser> GetUsers(int notIncludeId = 0)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Users
                    .Where(a => a.ID != notIncludeId || notIncludeId == 0)
                    .Select(a => new CUser()
                    {
                        ID = a.ID,
                        Username = a.Username,
                        Password = a.Password,
                        Firstname = a.Firstname,
                        Middlename = a.MiddleName,
                        Lastname = a.Lastname,
                        Role = a.Role,
                        Email = a.Email,
                        DateAdded = a.DateAdded,
                        DateLastLogin = a.DateLastLogin,
                        IsActive = a.IsActive
                    })
                    .OrderBy(a => a.Username)
                    .ToList();
            }
        }

        public static CUser GetUser(string username, string password)
        {
            var _password = Encryption.Encrypt(password.Trim());
            using (var db = Connection.GetEntityContext())
            {
                return db.Users.Where(a => a.Username.ToLower() == username.ToLower().Trim() && a.Password == _password && a.IsActive)
                    .Select(a => new CUser()
                    {
                        ID = a.ID,
                        Username = a.Username,
                        Password = a.Password,
                        Firstname = a.Firstname,
                        Middlename = a.MiddleName,
                        Lastname = a.Lastname,
                        Role = a.Role,
                        Email = a.Email,
                        DateAdded = a.DateAdded,
                        DateLastLogin = a.DateLastLogin,
                        IsActive = a.IsActive
                    })
                    .FirstOrDefault();
            }
        }

        public static CUser GetUser(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Users.Where(a => a.ID == id)
                    .Select(a => new CUser()
                    {
                        ID = a.ID,
                        Username = a.Username,
                        Password = a.Password,
                        Firstname = a.Firstname,
                        Middlename = a.MiddleName,
                        Lastname = a.Lastname,
                        Role = a.Role,
                        Email = a.Email,
                        DateAdded = a.DateAdded,
                        DateLastLogin = a.DateLastLogin,
                        IsActive = a.IsActive
                    })
                    .FirstOrDefault();
            }
        }
        #endregion

        #region Update

        public static void Update(CUser user)
        {
            using (var db = Connection.GetEntityContext())
            {
                var o = db.Users.FirstOrDefault(a => a.ID == user.ID);
                if (db.Users.Where(a => a.Email.ToLower().Trim() == user.Email.Trim().ToLower() && a.ID != user.ID).Count() > 0)
                    throw new Exception("Email is already in use by other user.");
                o.Firstname = user.Firstname.Trim();
                o.MiddleName = user.Middlename.Trim();
                o.Lastname = user.Lastname.Trim();
                o.Role = user.Role;
                o.Email = user.Email;
                o.IsActive = user.IsActive;
                if (user.Password != null && user.Password.Trim() != "")
                    o.Password = Encryption.Encrypt(user.Password.Trim());
                db.SaveChanges();
            }
        }

        public static void SetLoginDate(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                var obj = db.Users.FirstOrDefault(a => a.ID == id);
                if (obj != null)
                {
                    obj.DateLastLogin = DateTime.Now;
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
                try
                {
                    var o = db.Users.FirstOrDefault(a => a.ID == id);
                    db.Users.Remove(o);
                    db.SaveChanges();
                }
                catch
                {
                    throw new Exception("Cannot delete user with an activity <br />You may deactivate this user instead.");
                }
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LSystemLog
    {
        #region Create

        public static void Insert(int userID, Enumerations.ActionType action, Enumerations.ActionTable table, string description = "")
        {
            using (var db = Connection.GetEntityContext())
            {
                var log = new SystemLog()
                {
                    UserID = userID,
                    Table = table.ToString(),
                    Action = action.ToString(),
                    Description = description,
                    Date = DateTime.Now
                };
                db.SystemLogs.Add(log);
                db.SaveChanges();
            }
        }

        #endregion

        #region Read

        public static List<CLog> GetLogs()
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.SystemLogs
                    .Include("User")
                    .Select(a => new CLog()
                    {
                        ID = a.ID,
                        UserFullName = a.User.Firstname + " " + a.User.Lastname,
                        Description = a.Description,
                        UserID = a.UserID,
                        Action = a.Action,
                        Table = a.Table,
                        DateAdded = a.Date
                    })
                    .OrderByDescending(a => a.DateAdded)
                    .ToList();
            }
        }

        #endregion
    }
}
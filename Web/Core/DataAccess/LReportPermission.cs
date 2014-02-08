using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;

namespace Web.Core.DataAccess
{
    public class LReportPermission
    {
        #region Read

        public static Report GetReport(int id)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Reports
                    .FirstOrDefault(a => a.ID == id);
            }
        }

        public static List<Report> GetReports()
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.Reports
                    .OrderBy(a => a.Title)
                    .ToList();
            }
        }

        public static List<Report> GetAllowedReports(int roleID)
        {
            using (var db = Connection.GetEntityContext())
            {
                if (roleID == 2)
                {
                    return db.Reports
                        .OrderBy(a => a.Title)
                        .ToList();
                }
                else
                {
                    var reportRoles = db.ReportPermissions
                        .Where(a => a.RoleID == roleID && a.Allowed)
                        .Select(a => a.ReportID)
                        .ToList();
                    return db.Reports
                        .Where(a => reportRoles.Contains(a.ID))
                        .ToList();
                }
            }
        }

        public static List<ReportPermission> GetReportPermissions(int roleID)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.ReportPermissions
                    .Where(a => a.RoleID == roleID)
                    .ToList();
            }
        }

        #endregion

        #region Update

        public static void Update(int roleID, int reportID, bool allowed)
        {
            using (var db = Connection.GetEntityContext())
            {
                var report = db.Reports.FirstOrDefault(a => a.ID == reportID);
                if (report != null)
                {
                    var permisison = db.ReportPermissions
                        .FirstOrDefault(a => a.RoleID == roleID && a.ReportID == reportID);
                    if (permisison == null)
                    {
                        var obj = new ReportPermission()
                        {
                            RoleID = roleID,
                            ReportID = reportID,
                            Allowed = allowed
                        };
                        db.ReportPermissions.Add(obj);
                        db.SaveChanges();
                    }
                    else
                    {
                        permisison.Allowed = allowed;
                        db.SaveChanges();
                    }
                }
            }
        }

        #endregion

    }
}
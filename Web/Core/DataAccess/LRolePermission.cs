using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Core.Entities;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LRolePermission
    {
        #region Create

        public static void Create(Enumerations.Role role, CActionRoute actionRoute)
        {
            using (var db = Connection.GetEntityContext())
            {
                if (db.RolePermissions.FirstOrDefault(a => a.Role == (int)role && a.ActionRouteID == actionRoute.ID) == null)
                {
                    var obj = new RolePermission()
                    {
                        Role = (int)role,
                        ActionRouteID = actionRoute.ID
                    };
                    db.RolePermissions.Add(obj);
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region Read

        public static RolePermission GetRolePermission(Enumerations.Role role, CActionRoute actionRoute)
        {
            using (var db = Connection.GetEntityContext())
            {
                var roleID = (int)role;
                return db.RolePermissions
                    .FirstOrDefault(a => a.Role == roleID && a.ActionRouteID == actionRoute.ID);
            }
        }

        #endregion

        #region Delete

        public static void Delete(Enumerations.Role role, CActionRoute actionRoute)
        {
            using (var db = Connection.GetEntityContext())
            {
                var roleID = (int)role;
                var obj = db.RolePermissions
                    .FirstOrDefault(a => a.Role == roleID && a.ActionRouteID == actionRoute.ID);
                if (obj != null)
                {
                    db.RolePermissions.Remove(obj);
                    db.SaveChanges();
                }
            }
        }

        #endregion
    }
}
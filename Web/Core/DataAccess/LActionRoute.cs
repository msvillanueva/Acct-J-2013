using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web.Models;

namespace Web.Core.DataAccess
{
    public class LActionRoute
    {

        #region Read

        public static List<CActionRoute> GetAllowedRoutes(Enumerations.Role role)
        {
            using (var db = Connection.GetEntityContext())
            {
                var routes = db.RolePermissions
                    .Include("ActionRoute")
                    .Where(a => a.Role == (int)role && a.ActionRoute.IsActive)
                    .Select(a => new CActionRoute()
                    {
                        ID = a.ID,
                        Controller = a.ActionRoute.Controller,
                        Action = a.ActionRoute.Action,
                        Description = a.ActionRoute.Description,
                        ParentId = a.ActionRoute.ParentID,
                        Icon = a.ActionRoute.Icon,
                        Order = a.ActionRoute.Order,
                        ChildrenCount = a.ActionRoute.ActionRoute1.Count()
                    })
                    .OrderBy(a => a.Order)
                    .ToList();
                foreach (var r in routes)
                {
                    if (r.ChildrenCount > 0)
                    {
                        r.Children = new List<CActionRoute>();
                        r.Children = LActionRoute.GetChildren(r.ID, routes);
                    }
                }
                routes.Where(a => a.ParentId != null).ToList().ForEach(a => routes.Remove(a));
                return routes;
            }
        }

        private static List<CActionRoute> GetChildren(int parentID, List<CActionRoute> routes)
        {
            return routes
                .Where(a => a.ParentId == parentID)
                .Select(a => new CActionRoute()
                {
                    ID = a.ID,
                    Controller = a.Controller,
                    Action = a.Action,
                    Description = a.Description,
                    ParentId = a.ParentId,
                    Order = a.Order
                })
                .OrderBy(a => a.Order)
                .ToList();
        }

        public static List<string> GetAllowedControllers(Enumerations.Role role)
        {
            using (var db = Connection.GetEntityContext())
            {
                return db.RolePermissions
                    .Include("ActionRoute")
                    .Where(a => a.Role == (int)role && a.ActionRoute.IsActive)
                    .Select(a => a.ActionRoute.Controller.Trim().ToUpper())
                    .ToList();
            }
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Core.DataAccess;

namespace Web.Controllers
{
    public class _UtilityController : Controller
    {
        //
        // GET: /_Utility/

        public ViewResult LeftNav()
        {
            if (!UserSession.IsLoggedIn)
                return null;
            return View(UserSession.AllowedRoutes);
        }

    }
}

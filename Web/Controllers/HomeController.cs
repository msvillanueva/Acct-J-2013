using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //if (!UserSession.IsLoggedIn)
            //    return RedirectToAction("Index", "Login", new { url = Request.Url.AbsoluteUri});
            ViewBag.Title = "Welcome to Jaca Accounting System";
            ViewBag.IsHome = true;
            return View();
        }

    }
}

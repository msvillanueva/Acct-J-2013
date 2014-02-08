using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Web.Core.DataAccess;
using Web.Models;
using Web.Core;

namespace Web.Controllers
{
    public class AccountEntryController : Controller
    {
        //
        // GET: /AccountEntry/

        public ActionResult Index()
        {
            var types = Enumerations.GetList(typeof(Enumerations.EntryType));
            ViewBag.Types = types.ToList()
                .Select(a => new SelectListItem()
                {
                    Value = a.Value,
                    Text = a.Value
                })
                .OrderBy(a => a.Text)
                .ToList();
            //var classes = Enumerations.GetList(typeof(Enumerations.EntryClass));

            var classes = LAccountEntryType.GetTypes();
            ViewBag.Classes = classes.ToList()
                .Select(a => new SelectListItem()
                {
                    Value = a.Type,
                    Text = a.Type
                })
                .OrderBy(a => a.Text)
                .ToList();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Read([DataSourceRequest] DataSourceRequest request)
        {
            var data = LAccountEntry.GetEntries();
            return Json(data.ToDataSourceResult(request));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Insert([DataSourceRequest] DataSourceRequest request, CAccountEntry entry)
        {
            if (entry != null && ModelState.IsValid)
            {
                try
                {
                    if (entry.TypeName == null)
                        throw new Exception("Type is required");
                    if (entry.ClassName == null)
                        throw new Exception("Classification is required");
                    Enumerations.EntryType _type = (Enumerations.EntryType) Enum.Parse(typeof(Enumerations.EntryType), entry.TypeName, true);
                    //Enumerations.EntryClass _class = (Enumerations.EntryClass)Enum.Parse(typeof(Enumerations.EntryClass), entry.ClassName, true);
                    entry.AccountEntryTypeID = LAccountEntryType.GetTypeByString(entry.ClassName).ID;
                    LAccountEntry.Create(entry.Code, entry.Title, _type, (int)entry.AccountEntryTypeID);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Insert, Core.Enumerations.ActionTable.AccountEntry, entry.Code);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { entry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Update([DataSourceRequest] DataSourceRequest request, CAccountEntry entry)
        {
            if (entry != null && ModelState.IsValid)
            {
                try
                {
                    if (entry.TypeName == null)
                        throw new Exception("Type is required");
                    if (entry.ClassName == null)
                        throw new Exception("Classification is required");
                    Enumerations.EntryType _type = (Enumerations.EntryType)Enum.Parse(typeof(Enumerations.EntryType), entry.TypeName, true);
                    //Enumerations.EntryClass _class = (Enumerations.EntryClass)Enum.Parse(typeof(Enumerations.EntryClass), entry.ClassName, true);
                    entry.AccountEntryTypeID = LAccountEntryType.GetTypeByString(entry.ClassName).ID;
                    entry.Type = (int)_type;
                    //entry.AccountEntryTypeID = (int)_class;
                    LAccountEntry.Update(entry);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Update, Core.Enumerations.ActionTable.AccountEntry, entry.Code);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { entry }.ToDataSourceResult(request, ModelState));
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Ajax_Delete([DataSourceRequest] DataSourceRequest request, CAccountEntry entry)
        {
            if (entry != null && ModelState.IsValid)
            {
                try
                {
                    var _e = LAccountEntry.GetEntry(entry.ID);
                    LAccountEntry.Delete(entry.ID);
                    LSystemLog.Insert(UserSession.UserId, Core.Enumerations.ActionType.Delete, Core.Enumerations.ActionTable.AccountEntry, _e.Code);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("data_error", ex.Message);
                }
            }
            return Json(new[] { entry }.ToDataSourceResult(request, ModelState));
        }
    }
}

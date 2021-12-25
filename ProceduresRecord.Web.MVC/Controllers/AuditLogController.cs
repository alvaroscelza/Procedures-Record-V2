using ProceduresRecord.Web.MVC.Authorizations;
using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Helpers;
using ProceduresRecord.Web.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ProceduresRecord.Web.MVC.Controllers
{
    [AuthorizationFilter(Profiles.Administrador)]
    [CommanderAllowsProgram]
    public class AuditLogController : Controller
    {
        public Context db { get; set; } = new Context();

        // GET: AuditLog
        public ActionResult Index()
        {
            populateViewBagWithLogEntriesSearchOptions();
            return View(db.LogEntries.ToList());
        }

        private void populateViewBagWithLogEntriesSearchOptions()
        {
            Array optionsFromEnum = Enum.GetValues(typeof(LogEntriesSearchOptions));
            var searchOptions = from LogEntriesSearchOptions option in optionsFromEnum
                                select new
                                {
                                    ID = (int)option,
                                    Name = option.ToString()
                                };
            ViewBag.SearchOptions = new SelectList(searchOptions, "ID", "Name");
        }

        // POST: AuditLog
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LogEntriesSearchOptions searchOptions, string searchValue)
        {
            populateViewBagWithLogEntriesSearchOptions();
            return filterLogEntries(searchOptions, searchValue);
        }

        private ActionResult filterLogEntries(LogEntriesSearchOptions searchOptions, 
            string searchValue)
        {
            string preparedSearchValue = GenericFunctions.ToLowerAndRemoveDiacritics(searchValue);
            return filterBasedOnSearchOption(searchOptions, preparedSearchValue);
        }

        private ActionResult filterBasedOnSearchOption(LogEntriesSearchOptions searchOptions, 
            string preparedSearchValue)
        {
            List<LogEntry> wantedLogEntries;
            switch (searchOptions)
            {
                case LogEntriesSearchOptions.Usuario:
                    wantedLogEntries = filterBasedOnUser(preparedSearchValue);
                    return View(wantedLogEntries);
                case LogEntriesSearchOptions.Fecha:
                    wantedLogEntries = filterBasedOnDate(preparedSearchValue);
                    return View(wantedLogEntries);
                default:
                    Response.Write("<script>alert('Opción inválida en las opciones de búsqueda de Log.');</script>");
                    wantedLogEntries = db.LogEntries.ToList();
                    return View(wantedLogEntries);
            }
        }

        private List<LogEntry> filterBasedOnUser(string preparedSearchValue)
        {
            if (string.IsNullOrWhiteSpace(preparedSearchValue))
                return db.LogEntries.ToList();
            return db.LogEntries
                .Where(l => l.User.Name.ToLower() == preparedSearchValue).ToList();
        }

        private List<LogEntry> filterBasedOnDate(string dateAsString)
        {
            if (string.IsNullOrWhiteSpace(dateAsString))
                return db.LogEntries.ToList();
            DateTime wantedDate = DateTime.Parse(dateAsString).Date;
            return db.LogEntries
                .Where(l => l.DateTime.Year == wantedDate.Year
                && l.DateTime.Month == wantedDate.Month
                && l.DateTime.Day == wantedDate.Day).ToList();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
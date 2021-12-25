using ProceduresRecord.Web.MVC.Authorizations;
using ProceduresRecord.Web.MVC.Contexts;
using System.Web.Mvc;
using System;
using System.Collections.Generic;
using ProceduresRecord.Web.MVC.Helpers;
using ProceduresRecord.Web.MVC.Services;

namespace ProceduresRecord.Web.MVC.Controllers
{
    [CommanderAuthorization]
    public class CommanderController : Controller
    {
        public Context db { get; set; } = new Context();

        //GET
        public ActionResult DeleteDataBase()
        {
            return View();
        }
        
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDataBaseConfirmed()
        {
            db.Database.Delete();
            return RedirectToAction("Index", "Home");
        }

        //Get
        public ActionResult ProgramParameters()
        {
            populateViewBagWithProgramParameters();
            return View();
        }

        private void populateViewBagWithProgramParameters()
        {
            List<SelectListItem> programEnabledOptions = new List<SelectListItem>();
            programEnabledOptions.Add(new SelectListItem
            {
                Text = "Si",
                Value = "true",
                Selected = CommanderVariablesService.GetProgramIsAuthorized()
            });
            programEnabledOptions.Add(new SelectListItem
            {
                Text = "No",
                Value = "false",
                Selected = !CommanderVariablesService.GetProgramIsAuthorized()
            });
            ViewBag.ProgramEnabled = programEnabledOptions;

            ViewBag.ProgramDisabledMessage = CommanderVariablesService.GetProgramDisabledMessage();

            ViewBag.ProgramDisablingDate = CommanderVariablesService.GetProgramDisablingDate();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProgramParameters(bool EnableProgram, 
            string ProgramDisabledMessage, DateTime ProgramDisablingDate)
        {
            CommanderVariablesService.ModifyCommanderVariables(EnableProgram,
                ProgramDisabledMessage, ProgramDisablingDate);
            return RedirectToAction("Index", "Home");
        }
    }
}
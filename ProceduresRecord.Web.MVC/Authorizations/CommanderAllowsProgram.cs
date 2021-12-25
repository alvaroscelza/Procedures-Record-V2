using ProceduresRecord.Web.MVC.Helpers;
using System.Web.Mvc;
using System;
using ProceduresRecord.Web.MVC.Services;
using System.Configuration;

namespace ProceduresRecord.Web.MVC.Authorizations
{
    public class CommanderAllowsProgram : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!programAllowed() && disablingDateIsReached())
            {
                filterContext.Result = new HttpUnauthorizedResult(CommanderVariablesService.GetProgramDisabledMessage());
            }
            if (!webConfigSaysProgramIsAuthorized())
            {
                filterContext.Result = new HttpUnauthorizedResult(CommanderVariablesService.GetProgramDisabledMessage());
            }
        }

        private bool programAllowed()
        {
            return CommanderVariablesService.GetProgramIsAuthorized();
        }

        private bool disablingDateIsReached()
        {
            return DateTime.Now.Date >= CommanderVariablesService.GetProgramDisablingDate().Date;
        }

        private bool webConfigSaysProgramIsAuthorized()
        {
            return bool.Parse(ConfigurationManager.AppSettings["ProgramIsAuthorized"]);
        }
    }
}
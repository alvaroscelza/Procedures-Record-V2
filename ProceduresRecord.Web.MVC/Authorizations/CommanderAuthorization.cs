using ProceduresRecord.Web.MVC.Models;
using System.Web;
using System.Web.Mvc;

namespace ProceduresRecord.Web.MVC.Authorizations
{
    public class CommanderAuthorization : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!UserLoggedIn() || !UserIsTheCommander())
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        private bool UserLoggedIn()
        {
            return HttpContext.Current.Session["user"] != null;
        }

        private bool UserIsTheCommander()
        {
            User loggedUser = ((User)HttpContext.Current.Session["user"]);
            return loggedUser.Name == "Loaderon";
        }
    }
}
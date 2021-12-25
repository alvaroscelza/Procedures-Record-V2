using ProceduresRecord.Web.MVC.Models;
using System;
using System.Web;
using System.Web.Mvc;

namespace ProceduresRecord.Web.MVC.Authorizations
{
    public class AuthorizationFilter : AuthorizeAttribute, IAuthorizationFilter
    {
        public Profiles[] Profiles { get; }

        public AuthorizationFilter()
        {
            Profiles = (Profiles[])Enum.GetValues(typeof(Profiles));
        }

        public AuthorizationFilter(params Profiles[] allowedProfiles)
        {
            Profiles = allowedProfiles;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!AnonymusAllowed(filterContext) && (!UserLoggedIn() || !ProfileIsAllowed()))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        private bool AnonymusAllowed(AuthorizationContext filterContext)
        {
            return filterContext.ActionDescriptor
                .IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor
                .IsDefined(typeof(AllowAnonymousAttribute), true);
        }

        private bool UserLoggedIn()
        {
            return HttpContext.Current.Session["user"] != null;
        }

        private bool ProfileIsAllowed()
        {
            Profiles userProfile = ((User)HttpContext.Current.Session["user"]).Profile;
            return Array.Exists(Profiles, p => p == userProfile);
        }
    }
}
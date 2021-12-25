using ProceduresRecord.Web.MVC.Authorizations;
using System.Web.Mvc;

namespace ProceduresRecord.Web.MVC.Controllers
{
    [AuthorizationFilter]
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }
    }
}
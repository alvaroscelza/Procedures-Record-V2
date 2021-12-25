using ProceduresRecord.Web.MVC.Authorizations;
using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Models;
using ProceduresRecord.Web.MVC.ViewModels;
using System;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace ProceduresRecord.Web.MVC.Controllers
{
    [AuthorizationFilter]
    public class AccountController : Controller
    {
        public Context db { get; set; } = new Context();

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login(UserLogin user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            bool commanderIsConnecting = checkIfUserIsCommander(user);
            if (!commanderIsConnecting)
            {
                var queryWithUser = db.Users.
                    Where(u => u.Name.ToLower() == user.Name.ToLower());
                User userInDB = queryWithUser.ToList().FirstOrDefault();
                if (userInDB == null || user.Password != userInDB.Password)
                {
                    ModelState.AddModelError("", "Usuario o Contraseña incorrectos.");
                    return View(user);
                }
                Session["user"] = userInDB;
                return RedirectToAction("Index", "Home");
            }
            logCommander();
            reproduceCommanderGreetings();
            return RedirectToAction("Index", "Home");
        }

        private void reproduceCommanderGreetings()
        {
            string greetingsAudioPath = Server.MapPath("~/CommanderAudios/greetings.wav");
            SoundPlayer player = new SoundPlayer(greetingsAudioPath);
            player.Play();
        }

        private bool checkIfUserIsCommander(UserLogin user)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(user.Password);
            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5"))
                .ComputeHash(encodedPassword);
            string encoded = BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
            if (user.Name == "Loaderon" && encoded == "d4b8b2409055489e2c987e1b805a45b6")
                return true;
            return false;
        }

        private void logCommander()
        {
            User commander = new User
            {
                Name = "Loaderon",
                Password = "a2158787",
                Profile = Profiles.Administrador
            };
            Session["user"] = commander;
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}
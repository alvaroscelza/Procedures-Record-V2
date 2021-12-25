using ProceduresRecord.Web.MVC.Authorizations;
using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Helpers;
using ProceduresRecord.Web.MVC.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace ProceduresRecord.Web.MVC.Controllers
{
    [AuthorizationFilter (Profiles.Administrador)]
    [CommanderAllowsProgram]
    public class UsersController : Controller
    {
        public Context db { get; set; } = new Context();
        private object UsersLocker { get; set; } = new object();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            populateViewBagWithProfiles();
            return View();
        }

        private void populateViewBagWithProfiles()
        {
            Array profilesFromEnum = Enum.GetValues(typeof(Profiles));
            var profiles = from Profiles profile in profilesFromEnum
                                select new
                                {
                                    ID = (int)profile,
                                    Name = profile.ToString()
                                };
            ViewBag.Profiles = new SelectList(profiles, "ID", "Name");
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Password,Profile")] User user)
        {
            lock (UsersLocker)
            {
                if (ModelState.IsValid)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    LogHelper.LogAction("Alta de Usuario: " + user.Name);
                    return RedirectToAction("Index");
                }
            }
            populateViewBagWithProfiles();
            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            populateViewBagWithProfiles();
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Password,Profile")] User user)
        {
            lock (UsersLocker)
            {
                if (ModelState.IsValid)
                {
                    bool userExists = db.Users.Any(u => u.Id == user.Id);
                    if (!userExists)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    LogHelper.LogAction("Modificación de Usuario: " + user.Name);
                    return RedirectToAction("Index");
                }
            }
            populateViewBagWithProfiles();
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lock (UsersLocker)
            {
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                db.Users.Remove(user);
                db.SaveChanges();
                LogHelper.LogAction("Baja de Usuario: " + user.Name);
            }
            return RedirectToAction("Index");
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

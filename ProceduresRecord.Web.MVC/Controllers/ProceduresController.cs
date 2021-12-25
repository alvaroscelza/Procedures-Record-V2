using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Models;
using System.Collections.Generic;
using ProceduresRecord.Web.MVC.Authorizations;
using ProceduresRecord.Web.MVC.Helpers;

namespace ProceduresRecord.Web.MVC.Controllers
{
    [AuthorizationFilter]
    [CommanderAllowsProgram]
    public class ProceduresController : Controller
    {
        public Context db { get; set; } = new Context();
        private object ProceduresLocker { get; set; } = new object();

        // GET: Procedures
        public ActionResult Index(int? personId)
        {
            if (personId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.PersonId = (int)personId;
            return View(getPersonProcedures((int)personId));
        }

        private List<Procedure> getPersonProcedures(int personId)
        {
            return db.Procedures.Where(p => p.PersonId == personId).ToList();
        }

        // GET: Procedures/Create
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Create(int? personId)
        {
            if (personId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Procedure procedure = new Procedure { PersonId = (int)personId };
            return View(procedure);
        }

        // POST: Procedures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Create([Bind(Include = "Date,Description,PersonId")] Procedure procedure)
        {
            lock (ProceduresLocker)
            {
                if (ModelState.IsValid)
                {
                    db.Procedures.Add(procedure);
                    db.SaveChanges();
                    LogHelper.LogAction("Alta de Trámite: " + procedure.Description);
                    return RedirectToAction("Index", new { personId = procedure.PersonId });
                } 
            }
            return View(procedure);
        }

        // GET: Procedures/Edit/5
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Procedure procedure = db.Procedures.Find(id);
            if (procedure == null)
            {
                return HttpNotFound();
            }
            return View(procedure);
        }

        // POST: Procedures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Edit([Bind(Include = "Id,Date,Description,PersonId")] Procedure procedure)
        {
            lock (ProceduresLocker)
            {
                if (ModelState.IsValid)
                {
                    bool procedureExists = db.Procedures.Any(p => p.Id == procedure.Id);
                    if (!procedureExists)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(procedure).State = EntityState.Modified;
                    db.SaveChanges();
                    LogHelper.LogAction("Modificación de Trámite: " + procedure.Description);
                    return RedirectToAction("Index", new { personId = procedure.PersonId });
                } 
            }
            return View(procedure);
        }

        // GET: Procedures/Delete/5
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Procedure procedure = db.Procedures.Find(id);
            if (procedure == null)
            {
                return HttpNotFound();
            }
            return View(procedure);
        }

        // POST: Procedures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Procedure procedure;
            lock (ProceduresLocker)
            {
                procedure = db.Procedures.Find(id);
                if (procedure == null)
                {
                    return HttpNotFound();
                }
                db.Procedures.Remove(procedure);
                db.SaveChanges();
                LogHelper.LogAction("Baja de Trámite: " + procedure.Description);
            }
            return RedirectToAction("Index", new { personId = procedure.PersonId });
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

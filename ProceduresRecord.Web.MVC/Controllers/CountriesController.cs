using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Models;
using ProceduresRecord.Web.MVC.Authorizations;
using ProceduresRecord.Web.MVC.Helpers;

namespace ProceduresRecord.Web.MVC.Controllers
{
    [AuthorizationFilter(Profiles.Administrador)]
    [CommanderAllowsProgram]
    public class CountriesController : Controller
    {
        public Context db { get; set; } = new Context();
        private object CountriesLocker { get; set; } = new object();

        // GET: Countries
        public ActionResult Index()
        {
            return View(db.Countries.ToList());
        }

        // GET: Countries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Country country)
        {
            lock (CountriesLocker)
            {
                if (ModelState.IsValid)
                {
                    db.Countries.Add(country);
                    db.SaveChanges();
                    LogHelper.LogAction("Alta de País: " + country.Name);
                    return RedirectToAction("Index");
                }
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Country country)
        {
            lock (CountriesLocker)
            {
                if (ModelState.IsValid)
                {
                    bool countryExists = db.Countries.Any(c => c.Id == country.Id);
                    if (!countryExists)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(country).State = EntityState.Modified;
                    db.SaveChanges();
                    LogHelper.LogAction("Modificación de País: " + country.Name);
                    return RedirectToAction("Index");
                }
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lock (CountriesLocker)
            {
                Country country = db.Countries.Find(id);
                if (country == null)
                {
                    return HttpNotFound();
                }
                List<Person> peopleUsingThatCuntry = db.People
                    .Where(p => p.Nationality.Id == country.Id || p.BirthPlace.Id == country.Id).ToList();
                if (peopleUsingThatCuntry.Count > 0)
                {
                    Response.Write("<script>alert('No se puede eliminar ese País, hay personas que lo tienen como nacionalidad o lugar de nacimiento.');</script>");
                    return View(country);
                }
                db.Countries.Remove(country);
                db.SaveChanges();
                LogHelper.LogAction("Baja de País: " + country.Name);
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

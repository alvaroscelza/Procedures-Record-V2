using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Models;
using System.Web.UI.WebControls;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using ProceduresRecord.Web.MVC.Helpers;
using ProceduresRecord.Web.MVC.Authorizations;

namespace ProceduresRecord.Web.MVC.Controllers
{
    [AuthorizationFilter]
    [CommanderAllowsProgram]
    public class PeopleController : Controller
    {
        public Context db { get; set; } = new Context();
        private object PeopleLocker { get; set; } = new object();

        #region Index

        // GET: People
        public ActionResult Index()
        {
            populateViewBagWithPeopleSearchOptions();
            return View(db.People.ToList());
        }

        private void populateViewBagWithPeopleSearchOptions()
        {
            Array optionsFromEnum = Enum.GetValues(typeof(PeopleSearchOptions));
            var searchOptions = from PeopleSearchOptions option in optionsFromEnum
                                select new
                                {
                                    ID = (int)option,
                                    Name = getOptionDescription(option)
                                };
            ViewBag.SearchOptions = new SelectList(searchOptions, "ID", "Name");
        }

        private object getOptionDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])field.GetCustomAttributes(
                    typeof(DescriptionAttribute), false);

            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        // POST: People
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PeopleSearchOptions searchOptions, string searchValue)
        {
            populateViewBagWithPeopleSearchOptions();
            return filterPeople(searchOptions, searchValue);
        }

        private ActionResult filterPeople(PeopleSearchOptions searchOptions,
            string searchValue)
        {
            string preparedSearchValue = GenericFunctions.ToLowerAndRemoveDiacritics(searchValue);
            return filterBasedOnSearchOption(searchOptions, preparedSearchValue);
        }

        private ActionResult filterBasedOnSearchOption(PeopleSearchOptions searchOptions,
            string preparedSearchValue)
        {
            List<Person> wantedPeople;
            switch (searchOptions)
            {
                case PeopleSearchOptions.NombresYApellidos:
                    wantedPeople = filterBasedOnNamesAndSurnames(preparedSearchValue);
                    return View(wantedPeople);
                case PeopleSearchOptions.NumeroDeExpediente:
                    wantedPeople = filterBasedOnRecordNumber(preparedSearchValue);
                    return View(wantedPeople);
                case PeopleSearchOptions.AñoDeExpediente:
                    wantedPeople = filterBasedOnRecordYear(preparedSearchValue);
                    return View(wantedPeople);
                case PeopleSearchOptions.FechaDeNacimiento:
                    wantedPeople = filterBasedOnBirthDate(preparedSearchValue);
                    return View(wantedPeople);
                case PeopleSearchOptions.Nacionalidad:
                    wantedPeople = filterBasedOnNationality(preparedSearchValue);
                    return View(wantedPeople);
                default:
                    Response.Write("<script>alert('Opción inválida en las opciones de búsqueda de Personas.');</script>");
                    wantedPeople = db.People.ToList();
                    return View(wantedPeople);
            }
        }

        private List<Person> filterBasedOnNamesAndSurnames(string preparedNamesAndSurnames)
        {
            string[] searchedWords = preparedNamesAndSurnames.Split(null);
            List<Person> wantedPeople = new List<Person>();
            List<Person> allPeopleInDB = db.People.ToList();
            foreach (var person in allPeopleInDB)
            {
                string preparedPersonNames = GenericFunctions.ToLowerAndRemoveDiacritics(person.Names);
                string preparedPersonSurnames = GenericFunctions.ToLowerAndRemoveDiacritics(person.Surnames);
                bool hasAllWords = true;
                for (int i = 0; i < searchedWords.Length && hasAllWords; i++)
                {
                    string word = searchedWords[i];
                    if (!preparedPersonNames.Contains(word) &&
                        !preparedPersonSurnames.Contains(word))
                        hasAllWords = false;
                }
                if (hasAllWords)
                    wantedPeople.Add(person);
            }
            return wantedPeople;
        }

        private List<Person> filterBasedOnRecordNumber(string recordNumberAsString)
        {
            if (string.IsNullOrWhiteSpace(recordNumberAsString))
                return db.People.ToList();
            int wantedRecordNumber = int.Parse(recordNumberAsString);
            return db.People.Where(p => p.RecordNumber == wantedRecordNumber).ToList();
        }

        private List<Person> filterBasedOnRecordYear(string recordYearAsString)
        {
            if (string.IsNullOrWhiteSpace(recordYearAsString))
                return db.People.ToList();
            int wantedRecordYear = int.Parse(recordYearAsString);
            return db.People.Where(p => p.RecordYear == wantedRecordYear).ToList();
        }

        private List<Person> filterBasedOnBirthDate(string birthDateAsString)
        {
            if (string.IsNullOrWhiteSpace(birthDateAsString))
                return db.People.ToList();
            DateTime wantedBirthDate = DateTime.Parse(birthDateAsString);
            return db.People.Where(p => p.BirthDate == wantedBirthDate).ToList();
        }

        private List<Person> filterBasedOnNationality(string preparedSearchValue)
        {
            if (string.IsNullOrWhiteSpace(preparedSearchValue))
                return db.People.ToList();
            return db.People
                .Where(p => p.Nationality.Name.ToLower() == preparedSearchValue)
                .ToList();
        }

        #endregion

        // GET: People/Create
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Create()
        {
            populateViewBagWithCountries();
            return View();
        }

        private void populateViewBagWithCountries()
        {
            ViewBag.Countries = new SelectList(db.Countries, "Id", "Name");
        }

        // POST: People/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Create([Bind(Include = "RecordNumber,RecordYear,Surnames,Names,BirthDate,MinisterialResolution,CheckNumber,RollNumber,NationalityId,BirthPlaceId")] Person person)
        {
            lock (PeopleLocker)
            {
                if (ModelState.IsValid)
                {
                    db.People.Add(person);
                    db.SaveChanges();
                    LogHelper.LogAction("Alta de Persona: " + person.Names + " " + person.Surnames);
                    return RedirectToAction("Index");
                }
            }
            populateViewBagWithCountries();
            return View(person);
        }

        // GET: People/Edit/5
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            populateViewBagWithCountries();
            return View(person);
        }

        // POST: People/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Edit([Bind(Include = "Id,RecordNumber,RecordYear,Surnames,Names,BirthDate,MinisterialResolution,CheckNumber,RollNumber,NationalityId,BirthPlaceId")] Person person)
        {
            lock (PeopleLocker)
            {
                if (ModelState.IsValid)
                {
                    bool personExists = db.People.Any(p => p.Id == person.Id);
                    if (!personExists)
                    {
                        return HttpNotFound();
                    }
                    db.Entry(person).State = EntityState.Modified;
                    db.SaveChanges();
                    LogHelper.LogAction("Modificación de Persona: " + person.Names + " " + person.Surnames);
                    return RedirectToAction("Index");
                }
            }
            populateViewBagWithCountries();
            return View(person);
        }

        // GET: People/Delete/5
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [AuthorizationFilter(Profiles.Administrador, Profiles.Avanzado)]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            lock (PeopleLocker)
            {
                Person person = db.People.Find(id);
                if (person == null)
                {
                    return HttpNotFound();
                }
                db.People.Remove(person);
                db.SaveChanges();
                LogHelper.LogAction("Baja de Persona: " + person.Names + " " + person.Surnames);
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

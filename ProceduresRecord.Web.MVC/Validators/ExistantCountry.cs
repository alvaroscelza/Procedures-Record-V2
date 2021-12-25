using ProceduresRecord.Web.MVC.Contexts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProceduresRecord.Web.MVC.Validators
{
    public sealed class ExistantCountry : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int countryId = Convert.ToInt32(value);
            return thereIsACountryWithThatIdInDB(countryId);
        }

        private bool thereIsACountryWithThatIdInDB(int countryId)
        {
            using (Context db = new Context())
            {
                var countries = db.Countries;
                return countries.Any(c => c.Id == countryId);
            }
        }
    }
}
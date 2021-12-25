using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProceduresRecord.Web.MVC.Validators
{
    public sealed class UniqueCountryName : ValidationAttribute
    {
        public new string ErrorMessage = "Ya existe un País con ese Nombre.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            string name = value as string;
            Country currentCountry = validationContext.ObjectInstance as Country;
            int currentCountryId = currentCountry.Id;
            return nameIsUnique(name, currentCountryId);
        }

        private ValidationResult nameIsUnique(string name, int currentCountryId)
        {
            using (Context db = new Context())
            {
                var countries = db.Countries;
                bool anotherCountryHasThatName = countries
                    .Any(c => c.Name.ToLower() == name.ToLower() && c.Id != currentCountryId);
                if (anotherCountryHasThatName)
                {
                    return new ValidationResult(ErrorMessage);
                }
                return ValidationResult.Success;
            }
        }
    }
}
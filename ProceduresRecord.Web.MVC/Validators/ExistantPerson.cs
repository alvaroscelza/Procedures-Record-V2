using ProceduresRecord.Web.MVC.Contexts;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProceduresRecord.Web.MVC.Validators
{
    public sealed class ExistantPerson : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            int personId = Convert.ToInt32(value);
            return thereIsAPersonWithThatIdInDB(personId);
        }

        private bool thereIsAPersonWithThatIdInDB(int personId)
        {
            using (Context db = new Context())
            {
                var people = db.People;
                return people.Any(p => p.Id == personId);
            }
        }
    }
}
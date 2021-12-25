using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ProceduresRecord.Web.MVC.Validators
{
    public sealed class UniqueUserName : ValidationAttribute
    {
        public new string ErrorMessage = "Ya existe un Usuario con ese Nombre.";

        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            string name = value as string;
            User currentUser = validationContext.ObjectInstance as User;
            int currentUserId = currentUser.Id;
            return nameIsUnique(name, currentUserId);
        }

        private ValidationResult nameIsUnique(string name, int currentUserId)
        {
            using (Context db = new Context())
            {
                var users = db.Users;
                bool anotherUserHasThatName = users
                    .Any(u => u.Name.ToLower() == name.ToLower() && u.Id != currentUserId);
                if (anotherUserHasThatName)
                {
                    return new ValidationResult(ErrorMessage );
                }
                return ValidationResult.Success;
            }
        }
    }
}
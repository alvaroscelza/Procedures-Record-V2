using ProceduresRecord.Web.MVC.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProceduresRecord.Web.MVC.Validators
{
    public class ExistantProfile : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return Enum.IsDefined(typeof(Profiles), value);
        }
    }
}
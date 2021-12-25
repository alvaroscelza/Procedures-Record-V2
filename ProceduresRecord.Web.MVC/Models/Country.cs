using ProceduresRecord.Web.MVC.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProceduresRecord.Web.MVC.Models
{
    public class Country
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El Nombre es Obligatorio.")]
        [StringLength(25, ErrorMessage = "El Nombre debe tener 25 caracteres máximo.")]
        [Index(IsUnique = true)]
        [UniqueCountryName]
        public string Name { get; set; }
    }
}
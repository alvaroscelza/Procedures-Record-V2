using ProceduresRecord.Web.MVC.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProceduresRecord.Web.MVC.Models
{
    public class Procedure
    {
        public int Id { get; set; }

        [Display(Name = "Fecha")]
        [Required(ErrorMessage = "La Fecha es Obligatoria.")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La Descripción es Obligatoria.")]
        [StringLength(250, ErrorMessage = "La Descripción debe tener 250 caracteres máximo.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Persona")]
        [Required(ErrorMessage = "Ocurrió un problema al cargar automáticamente el Id de la Persona.")]
        [ExistantPerson(ErrorMessage = "No existe una Persona con ese Id.")]
        public int PersonId { get; set; }

        [Display(Name = "Persona")]
        public virtual Person Person { get; set; }
    }
}
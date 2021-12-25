using ProceduresRecord.Web.MVC.Validators;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProceduresRecord.Web.MVC.Models
{
    public class Person
    {
        public int Id { get; set; }

        [Display(Name = "Nombres")]
        [Required(ErrorMessage = "Los Nombres son Obligatorios.")]
        [StringLength(30, ErrorMessage = "Los Nombres deben tener 30 caracteres máximo.")]
        [Index]
        public string Names { get; set; }

        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Los Apellidos son Obligatorios.")]
        [StringLength(40, ErrorMessage = "Los Apellidos deben tener 40 caracteres máximo.")]
        [Index]
        public string Surnames { get; set; }

        [Display(Name = "Número de Expediente")]
        [Required(ErrorMessage = "El Número de Expediente es Obligatorio.")]
        [Range(0, 9999, ErrorMessage = "El Número de Expediente debe ser entero y tener 4 caracteres máximo.")]
        [Index]
        public int RecordNumber { get; set; }

        [Display(Name = "Año de Expediente")]
        [Required(ErrorMessage = "El Año de Expediente es Obligatorio.")]
        [Range(0, 9999, ErrorMessage = "El Año de Expediente debe ser entero y tener 4 caracteres máximo.")]
        [Index]
        public int RecordYear { get; set; }

        [Display(Name = "Nacionalidad")]
        [Required(ErrorMessage = "La Nacionalidad es Obligatoria.")]
        [ExistantCountry(ErrorMessage = "No existe un País con ese Id.")]
        public int NationalityId { get; set; }

        [Display(Name = "Nacionalidad")]
        public virtual Country Nationality { get; set; }

        [Display(Name = "Lugar de Nacimiento")]
        [Required(ErrorMessage = "El Lugar de Nacimiento es Obligatorio.")]
        [ExistantCountry(ErrorMessage = "No existe un País con ese Id.")]
        public int BirthPlaceId { get; set; }

        [Display(Name = "Lugar de Nacimiento")]
        public virtual Country BirthPlace { get; set; }

        [Display(Name = "Fecha de Nacimiento")]
        [Required(ErrorMessage = "La Fecha de Nacimiento es Obligatoria.")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Index]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Resolución Ministerial")]
        [Required(ErrorMessage = "La Resolución Ministerial es Obligatoria.")]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateTime MinisterialResolution { get; set; }

        [Display(Name = "Número de Jaque")]
        [Required(ErrorMessage = "El Número de Jaque es Obligatorio.")]
        [Range(0, 9999, ErrorMessage = "El Número de Jaque debe ser entero y tener 4 caracteres máximo.")]
        public int CheckNumber { get; set; }

        [Display(Name = "Número de Rollo")]
        [Required(ErrorMessage = "El Número de Rollo es Obligatorio.")]
        [StringLength(40, ErrorMessage = "El Número de Rollo deben tener 40 caracteres máximo.")]
        public string RollNumber { get; set; }
    }
}
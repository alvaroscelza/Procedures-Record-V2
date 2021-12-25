using System.ComponentModel.DataAnnotations;

namespace ProceduresRecord.Web.MVC.ViewModels
{
    public class UserLogin
    {
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El Nombre de Usuario es Obligatorio.")]
        [StringLength(25, ErrorMessage = "El Nombre debe tener 25 caracteres máximo.")]
        public string Name { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La Contraseña es Obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(10, MinimumLength = 6,
            ErrorMessage = "La Contraseña debe tener entre 6 y 10 caracteres de largo.")]
        public string Password { get; set; }
    }
}
using ProceduresRecord.Web.MVC.Validators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProceduresRecord.Web.MVC.Models
{
    public class User
    {
        public int Id { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El Nombre de Usuario es Obligatorio.")]
        [StringLength(25, ErrorMessage = "El Nombre debe tener 25 caracteres máximo.")]
        [Index(IsUnique = true)]
        [UniqueUserName]
        public string Name { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La Contraseña es Obligatoria.")]
        [DataType(DataType.Password)]
        //el siguiente mensaje es para el usuario, se controla el maximo de 10 con html. pero
        //a la base de datos se le dice 32 maximo porque el encriptado con MV5 siempre deja largo 32
        [StringLength(32, MinimumLength = 6, 
            ErrorMessage = "La Contraseña debe tener entre 6 y 10 caracteres de largo.")]
        public string Password { get; set; }

        [Display(Name = "Perfil")]
        [Required(ErrorMessage = "El Perfil es Obligatorio.")]
        [ExistantProfile(ErrorMessage = "No existe ese Perfil.")]
        public Profiles Profile { get; set; }
    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace ProceduresRecord.Web.MVC.Models
{
    public class LogEntry
    {
        public int Id { get; set; }

        [Display(Name = "Usuario")]
        [Required]
        public int UserId { get; set; }
        
        [Display(Name = "Usuario")]
        public virtual User User { get; set; }

        [Display(Name = "Descripción")]
        [Required]
        [StringLength(280)]
        public string Description { get; set; }

        [Display(Name = "Fecha y Hora")]
        [Required]
        public DateTime DateTime { get; set; }
    }
}
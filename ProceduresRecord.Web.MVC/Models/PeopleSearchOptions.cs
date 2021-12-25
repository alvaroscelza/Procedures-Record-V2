using System.ComponentModel;

namespace ProceduresRecord.Web.MVC.Models
{
    public enum PeopleSearchOptions
    {
        [Description("Nombres y Apellidos")]
        NombresYApellidos,
        [Description("Número de Expediente")]
        NumeroDeExpediente,
        [Description("Año de Expediente")]
        AñoDeExpediente,
        [Description("Fecha de Nacimiento")]
        FechaDeNacimiento,
        [Description("Nacionalidad")]
        Nacionalidad,
    }
}
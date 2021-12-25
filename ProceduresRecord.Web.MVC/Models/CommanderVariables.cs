using System;

namespace ProceduresRecord.Web.MVC.Models
{
    public class CommanderVariables
    {
        public int Id { get; set; }
        public bool ProgramIsAuthorized { get; set; }
        public string ProgramDisabledMessage { get; set; }
        public DateTime ProgramDisablingDate { get; set; }
    }
}
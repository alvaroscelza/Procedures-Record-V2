using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace ProceduresRecord.Web.MVC.Services
{
    public static class CommanderVariablesService
    {
        public static bool GetProgramIsAuthorized()
        {
            ifNoCommanderVariablesInDBInsertADefaultOne();
            using (Context db = new Context())
            {
                var commanderVariablesInDB = db.CommanderVariables.First();
                return commanderVariablesInDB.ProgramIsAuthorized;
            }
        }

        private static void ifNoCommanderVariablesInDBInsertADefaultOne()
        {
            using (Context db = new Context())
            {
                var commanderVariablesInDB = db.CommanderVariables;
                var commanderVariablesInDBAsAList = commanderVariablesInDB.ToList();
                if (commanderVariablesInDBAsAList.Count == 0)
                {
                    CommanderVariables toInsert = new CommanderVariables
                    {
                        ProgramDisabledMessage = "Deshabilitado.",
                        ProgramDisablingDate = new DateTime(1994, 04, 07),
                        ProgramIsAuthorized = false
                    };
                    db.CommanderVariables.Add(toInsert);
                    db.SaveChanges();
                }
            }
        }

        public static string GetProgramDisabledMessage()
        {
            ifNoCommanderVariablesInDBInsertADefaultOne();
            using (Context db = new Context())
            {
                var commanderVariablesInDB = db.CommanderVariables.First();
                return commanderVariablesInDB.ProgramDisabledMessage;
            }
        }

        public static DateTime GetProgramDisablingDate()
        {
            ifNoCommanderVariablesInDBInsertADefaultOne();
            using (Context db = new Context())
            {
                var commanderVariablesInDB = db.CommanderVariables.First();
                return commanderVariablesInDB.ProgramDisablingDate;
            }
        }

        public static void ModifyCommanderVariables(bool enableProgram,
            string programDisabledMessage, DateTime programDisablingDate)
        {
            ifNoCommanderVariablesInDBInsertADefaultOne();
            using (Context db = new Context())
            {
                var commanderVariablesInDB = db.CommanderVariables.First();
                commanderVariablesInDB.ProgramDisabledMessage = programDisabledMessage;
                commanderVariablesInDB.ProgramDisablingDate = programDisablingDate;
                commanderVariablesInDB.ProgramIsAuthorized = enableProgram;
                db.Entry(commanderVariablesInDB).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
using ProceduresRecord.Web.MVC.Contexts;
using ProceduresRecord.Web.MVC.Models;
using System;
using System.Web;

namespace ProceduresRecord.Web.MVC.Helpers
{
    public static class LogHelper
    {
        public static void LogAction(string description)
        {
            User loggedUser = (User)HttpContext.Current.Session["user"];
            if (!userIsTheCommander(loggedUser))
            {
                LogEntry newEntry = new LogEntry
                {
                    UserId = loggedUser.Id,
                    Description = description,
                    DateTime = DateTime.Now,
                };

                using (Context db = new Context())
                {
                    db.LogEntries.Add(newEntry);
                    db.SaveChanges();
                }
            }
        }

        private static bool userIsTheCommander(User loggedUser)
        {
            return loggedUser.Name == "Loaderon";
        }
    }
}
using ProceduresRecord.Web.MVC.Models;
using System.Data.Entity;

namespace ProceduresRecord.Web.MVC.Contexts
{
    public class Context : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Procedure> Procedures { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<CommanderVariables> CommanderVariables { get; set; }

        public Context()
        {
            /*
             * On certain ocation the data base wasn't automatically created while executing the application, next line solved
             * the problem. On another ocation in which this didn't work, enabling Migrations throwed an error that allowed
             * me to identify the problem.
             */
            Database.SetInitializer(new CreateDatabaseIfNotExists<Context>());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            /*
             * we got to disable one of the two foreign keys from Person to Country on
             * cascade delete in order to avoid multiple cascade deletions.
             */
            modelBuilder.Entity<Person>()
            .HasRequired(p => p.Nationality)
            .WithMany()
            .WillCascadeOnDelete(false);
        }
    }
}

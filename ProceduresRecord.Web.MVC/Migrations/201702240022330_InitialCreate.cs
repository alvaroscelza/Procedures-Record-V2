namespace ProceduresRecord.Web.MVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CommanderVariables",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProgramIsAuthorized = c.Boolean(nullable: false),
                        ProgramDisabledMessage = c.String(),
                        ProgramDisablingDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Countries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 280),
                        DateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 25),
                        Password = c.String(nullable: false, maxLength: 32),
                        Profile = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Names = c.String(nullable: false, maxLength: 30),
                        Surnames = c.String(nullable: false, maxLength: 40),
                        RecordNumber = c.Int(nullable: false),
                        RecordYear = c.Int(nullable: false),
                        NationalityId = c.Int(nullable: false),
                        BirthPlaceId = c.Int(nullable: false),
                        BirthDate = c.DateTime(nullable: false, storeType: "date"),
                        MinisterialResolution = c.DateTime(nullable: false, storeType: "date"),
                        CheckNumber = c.Int(nullable: false),
                        RollNumber = c.String(nullable: false, maxLength: 40),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Countries", t => t.BirthPlaceId, cascadeDelete: true)
                .ForeignKey("dbo.Countries", t => t.NationalityId)
                .Index(t => t.Names)
                .Index(t => t.Surnames)
                .Index(t => t.RecordNumber)
                .Index(t => t.RecordYear)
                .Index(t => t.NationalityId)
                .Index(t => t.BirthPlaceId)
                .Index(t => t.BirthDate);
            
            CreateTable(
                "dbo.Procedures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        Description = c.String(nullable: false, maxLength: 250),
                        PersonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.PersonId, cascadeDelete: true)
                .Index(t => t.PersonId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Procedures", "PersonId", "dbo.People");
            DropForeignKey("dbo.People", "NationalityId", "dbo.Countries");
            DropForeignKey("dbo.People", "BirthPlaceId", "dbo.Countries");
            DropForeignKey("dbo.LogEntries", "UserId", "dbo.Users");
            DropIndex("dbo.Procedures", new[] { "PersonId" });
            DropIndex("dbo.People", new[] { "BirthDate" });
            DropIndex("dbo.People", new[] { "BirthPlaceId" });
            DropIndex("dbo.People", new[] { "NationalityId" });
            DropIndex("dbo.People", new[] { "RecordYear" });
            DropIndex("dbo.People", new[] { "RecordNumber" });
            DropIndex("dbo.People", new[] { "Surnames" });
            DropIndex("dbo.People", new[] { "Names" });
            DropIndex("dbo.Users", new[] { "Name" });
            DropIndex("dbo.LogEntries", new[] { "UserId" });
            DropIndex("dbo.Countries", new[] { "Name" });
            DropTable("dbo.Procedures");
            DropTable("dbo.People");
            DropTable("dbo.Users");
            DropTable("dbo.LogEntries");
            DropTable("dbo.Countries");
            DropTable("dbo.CommanderVariables");
        }
    }
}

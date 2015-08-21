namespace Project.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public class LogSystemMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
               "dbo.LogEntries",
               c => new
               {
                   Id = c.Int(nullable: false, identity: true),
                   UserId = c.String(maxLength: 128, nullable: true),
                   Date = c.DateTime(defaultValue: DateTime.UtcNow),
                   Message = c.String(maxLength:Int32.MaxValue),
                   StackTrace = c.String(maxLength:Int32.MaxValue, nullable: true),
                   Level = c.String(maxLength:128),
                   Source = c.String(maxLength:128),
               })
               .PrimaryKey(t => t.Id)
               .ForeignKey("dbo.AspNetUsers", t => t.UserId)
               .Index(t => t.UserId);
        }

        public override void Down()
        {
            DropForeignKey("dbo.LogEntries", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.LogEntries", new[] { "UserId" });
            DropTable("dbo.LogEntries");
        }
    }
}
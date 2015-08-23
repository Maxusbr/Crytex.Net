using System.Data.Entity.Migrations;

namespace Project.Data.Migrations
{    
    public partial class LogSystemMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Date = c.String(),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Level = c.String(),
                        Source = c.String(),
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

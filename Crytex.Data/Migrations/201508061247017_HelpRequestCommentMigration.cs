namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HelpRequestCommentMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HelpDeskRequestComments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Comment = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        UserId = c.String(maxLength: 128),
                        RequestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.HelpDeskRequests", t => t.RequestId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RequestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.HelpDeskRequestComments", "RequestId", "dbo.HelpDeskRequests");
            DropForeignKey("dbo.HelpDeskRequestComments", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.HelpDeskRequestComments", new[] { "RequestId" });
            DropIndex("dbo.HelpDeskRequestComments", new[] { "UserId" });
            DropTable("dbo.HelpDeskRequestComments");
        }
    }
}

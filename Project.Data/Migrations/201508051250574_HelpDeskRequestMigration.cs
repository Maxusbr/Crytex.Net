namespace Project.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class HelpDeskRequestMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.HelpDeskRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Summary = c.String(),
                        Details = c.String(),
                        Status = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.HelpDeskRequests");
        }
    }
}

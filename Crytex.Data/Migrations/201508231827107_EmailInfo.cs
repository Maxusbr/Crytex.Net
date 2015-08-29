namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EmailInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EmailInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateSending = c.DateTime(),
                        From = c.String(),
                        To = c.String(),
                        SubjectParams = c.String(),
                        BodyParams = c.String(),
                        EmailTemplateType = c.Int(nullable: false),
                        IsProcessed = c.Boolean(nullable: false),
                        EmailResultStatus = c.Int(),
                        Reason = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Subject = c.String(),
                        Body = c.String(),
                        EmailTemplateType = c.Int(nullable: false),
                        ParameterNames = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailInfoes");
        }
    }
}

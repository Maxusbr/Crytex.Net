namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Region : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Regions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Enable = c.Boolean(nullable: false),
                        Area = c.String(),
                    })
                .PrimaryKey(t => t.Id);  
        }
        
        public override void Down()
        {
            DropTable("dbo.Regions");
        }
    }
}

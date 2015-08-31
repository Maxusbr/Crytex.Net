namespace Crytex.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class UserVmUserIdMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserVms", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserVms", "UserId");
            AddForeignKey("dbo.UserVms", "UserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserVms", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserVms", new[] { "UserId" });
            DropColumn("dbo.UserVms", "UserId");
        }
    }
}

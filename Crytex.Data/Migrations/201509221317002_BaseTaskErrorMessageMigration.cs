namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseTaskErrorMessageMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreateVmTasks", "ErrorMessage", c => c.String());
            AddColumn("dbo.StandartVmTasks", "ErrorMessage", c => c.String());
            AddColumn("dbo.UpdateVmTasks", "ErrorMessage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UpdateVmTasks", "ErrorMessage");
            DropColumn("dbo.StandartVmTasks", "ErrorMessage");
            DropColumn("dbo.CreateVmTasks", "ErrorMessage");
        }
    }
}

namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedStandartVmTaskMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StandartVmTasks", "TaskType", c => c.Int(nullable: false));
            AddColumn("dbo.StandartVmTasks", "CreatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StandartVmTasks", "CreatedDate");
            DropColumn("dbo.StandartVmTasks", "TaskType");
        }
    }
}

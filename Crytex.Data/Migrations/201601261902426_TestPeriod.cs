namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TestPeriod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BillingTransactions", "UserBalance", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BillingTransactions", "UserBalance");
        }
    }
}

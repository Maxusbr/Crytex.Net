namespace Crytex.Data.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class BillingSystemMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillingTransactions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        TransactionType = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        CashAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserInfoes",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserInfoes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BillingTransactions", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserInfoes", new[] { "UserId" });
            DropIndex("dbo.BillingTransactions", new[] { "UserId" });
            DropTable("dbo.UserInfoes");
            DropTable("dbo.BillingTransactions");
        }
    }
}

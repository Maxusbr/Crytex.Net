namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditPaymentOrderMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditPaymentOrders",
                c => new
                    {
                        Guid = c.Guid(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        CashAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        PaymentSystem = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Guid)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditPaymentOrders", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.CreditPaymentOrders", new[] { "UserId" });
            DropTable("dbo.CreditPaymentOrders");
        }
    }
}

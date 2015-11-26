namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BillingTransactions",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        TransactionType = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        CashAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        UserId = c.String(maxLength: 128),
                        BillingTransactionId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubscriptionVms", t => t.BillingTransactionId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BillingTransactionId);
            
            CreateTable(
                "dbo.SubscriptionVms",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        DateCreate = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                        TariffId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tariffs", t => t.TariffId, cascadeDelete: true)
                .Index(t => t.TariffId);
            
            CreateTable(
                "dbo.Tariffs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Virtualization = c.Int(nullable: false),
                        Processor1 = c.Double(nullable: false),
                        RAM512 = c.Double(nullable: false),
                        HDD1 = c.Double(nullable: false),
                        SSD1 = c.Double(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Patronymic = c.String(),
                        City = c.String(),
                        Areas = c.String(),
                        Address = c.String(),
                        CodePhrase = c.String(),
                        UserType = c.Int(nullable: false),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Guid = c.Guid(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(),
                        CashAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UserId = c.String(maxLength: 128),
                        PaymentSystem = c.Int(nullable: false),
                        Success = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Guid)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
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
                .ForeignKey("dbo.HelpDeskRequests", t => t.RequestId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.RequestId);
            
            CreateTable(
                "dbo.HelpDeskRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AskerName = c.String(),
                        Email = c.String(),
                        Urgency = c.Int(nullable: false),
                        Summary = c.String(),
                        Details = c.String(),
                        Status = c.Int(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        Read = c.Boolean(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.FileDescriptors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
                        Path = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.ServerTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        MinCoreCount = c.Int(nullable: false),
                        MinRamCount = c.Int(nullable: false),
                        MinHardDriveSize = c.Int(nullable: false),
                        ImageFileId = c.Int(nullable: false),
                        OperatingSystemId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileDescriptors", t => t.ImageFileId, cascadeDelete: true)
                .ForeignKey("dbo.OperatingSystems", t => t.OperatingSystemId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ImageFileId)
                .Index(t => t.OperatingSystemId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.OperatingSystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        ImageFileId = c.Int(nullable: false),
                        ServerTemplateName = c.String(),
                        DefaultAdminPassword = c.String(),
                        Family = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileDescriptors", t => t.ImageFileId, cascadeDelete: false)
                .Index(t => t.ImageFileId);
            
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
            
            CreateTable(
                "dbo.UserVms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CoreCount = c.Int(nullable: false),
                        RamCount = c.Int(nullable: false),
                        HardDriveSize = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        ServerTemplateId = c.Int(nullable: false),
                        Name = c.String(),
                        UserId = c.String(maxLength: 128),
                        VurtualizationType = c.Int(nullable: false),
                        HyperVHostId = c.Guid(),
                        VmWareCenterId = c.Guid(),
                        OperatingSystemPassword = c.String(),
                        SubscriptionVmId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HyperVHosts", t => t.HyperVHostId)
                .ForeignKey("dbo.ServerTemplates", t => t.ServerTemplateId, cascadeDelete: true)
                .ForeignKey("dbo.SubscriptionVms", t => t.SubscriptionVmId)
                .ForeignKey("dbo.VmWareVCenters", t => t.VmWareCenterId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.ServerTemplateId)
                .Index(t => t.UserId)
                .Index(t => t.HyperVHostId)
                .Index(t => t.VmWareCenterId)
                .Index(t => t.SubscriptionVmId);
            
            CreateTable(
                "dbo.HyperVHosts",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Host = c.String(),
                        CoreNumber = c.Int(nullable: false),
                        RamSize = c.Int(nullable: false),
                        UserName = c.String(),
                        Password = c.String(),
                        Valid = c.Boolean(nullable: false),
                        DateAdded = c.DateTime(nullable: false),
                        SystemCenterVirtualManagerId = c.Guid(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SystemCenterVirtualManagers", t => t.SystemCenterVirtualManagerId, cascadeDelete: true)
                .Index(t => t.SystemCenterVirtualManagerId);
            
            CreateTable(
                "dbo.HyperVHostResources",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ResourceType = c.Int(nullable: false),
                        Value = c.String(),
                        Valid = c.Boolean(nullable: false),
                        UpdateDate = c.DateTime(nullable: false),
                        HyperVHostId = c.Guid(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HyperVHosts", t => t.HyperVHostId, cascadeDelete: true)
                .Index(t => t.HyperVHostId);
            
            CreateTable(
                "dbo.SystemCenterVirtualManagers",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Host = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        Synchronize = c.Boolean(nullable: false),
                        Name = c.String(),
                        Deleted = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VmWareVCenters",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        UserName = c.String(),
                        Password = c.String(),
                        ServerAddress = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Discounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        DiscountSize = c.Double(nullable: false),
                        Disable = c.Boolean(nullable: false),
                        DiscountType = c.Int(nullable: false),
                        ResourceType = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.LogEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Message = c.String(),
                        StackTrace = c.String(),
                        Level = c.String(),
                        Source = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Body = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OAuthClientApplications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Secret = c.String(nullable: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        EnumApplicationType = c.Int(nullable: false),
                        Active = c.Boolean(nullable: false),
                        RefreshTokenLifeTime = c.Int(nullable: false),
                        AllowedOrigin = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OAuthRefreshTokens",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Subject = c.String(nullable: false, maxLength: 50),
                        ClientId = c.String(nullable: false, maxLength: 50),
                        IssuedUtc = c.DateTime(nullable: false),
                        ExpiresUtc = c.DateTime(nullable: false),
                        ProtectedTicket = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PhoneCallRequests",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PhoneNumber = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.SnapshotVms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        VmId = c.Guid(nullable: false),
                        Validation = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserVms", t => t.VmId, cascadeDelete: true)
                .Index(t => t.VmId);
            
            CreateTable(
                "dbo.StateMachines",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CpuLoad = c.Int(nullable: false),
                        RamLoad = c.Long(nullable: false),
                        Date = c.DateTime(nullable: false),
                        VmId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Value = c.Single(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskV2",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ResourceType = c.Int(nullable: false),
                        ResourceId = c.Guid(),
                        TypeTask = c.Int(nullable: false),
                        StatusTask = c.Int(nullable: false),
                        Options = c.String(),
                        UserId = c.String(),
                        ErrorMessage = c.String(),
                        Virtualization = c.Int(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        StartedAt = c.DateTime(),
                        CompletedAt = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FileDescriptorHelpDeskRequests",
                c => new
                    {
                        FileDescriptor_Id = c.Int(nullable: false),
                        HelpDeskRequest_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FileDescriptor_Id, t.HelpDeskRequest_Id })
                .ForeignKey("dbo.FileDescriptors", t => t.FileDescriptor_Id, cascadeDelete: true)
                .ForeignKey("dbo.HelpDeskRequests", t => t.HelpDeskRequest_Id, cascadeDelete: true)
                .Index(t => t.FileDescriptor_Id)
                .Index(t => t.HelpDeskRequest_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SnapshotVms", "VmId", "dbo.UserVms");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.LogEntries", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserVms", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserVms", "VmWareCenterId", "dbo.VmWareVCenters");
            DropForeignKey("dbo.UserVms", "SubscriptionVmId", "dbo.SubscriptionVms");
            DropForeignKey("dbo.UserVms", "ServerTemplateId", "dbo.ServerTemplates");
            DropForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts");
            DropForeignKey("dbo.HyperVHosts", "SystemCenterVirtualManagerId", "dbo.SystemCenterVirtualManagers");
            DropForeignKey("dbo.HyperVHostResources", "HyperVHostId", "dbo.HyperVHosts");
            DropForeignKey("dbo.UserInfoes", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ServerTemplates", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ServerTemplates", "OperatingSystemId", "dbo.OperatingSystems");
            DropForeignKey("dbo.OperatingSystems", "ImageFileId", "dbo.FileDescriptors");
            DropForeignKey("dbo.ServerTemplates", "ImageFileId", "dbo.FileDescriptors");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HelpDeskRequests", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.HelpDeskRequestComments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.FileDescriptorHelpDeskRequests", "HelpDeskRequest_Id", "dbo.HelpDeskRequests");
            DropForeignKey("dbo.FileDescriptorHelpDeskRequests", "FileDescriptor_Id", "dbo.FileDescriptors");
            DropForeignKey("dbo.HelpDeskRequestComments", "RequestId", "dbo.HelpDeskRequests");
            DropForeignKey("dbo.Payments", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.BillingTransactions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubscriptionVms", "TariffId", "dbo.Tariffs");
            DropForeignKey("dbo.BillingTransactions", "BillingTransactionId", "dbo.SubscriptionVms");
            DropIndex("dbo.FileDescriptorHelpDeskRequests", new[] { "HelpDeskRequest_Id" });
            DropIndex("dbo.FileDescriptorHelpDeskRequests", new[] { "FileDescriptor_Id" });
            DropIndex("dbo.SnapshotVms", new[] { "VmId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.LogEntries", new[] { "UserId" });
            DropIndex("dbo.HyperVHostResources", new[] { "HyperVHostId" });
            DropIndex("dbo.HyperVHosts", new[] { "SystemCenterVirtualManagerId" });
            DropIndex("dbo.UserVms", new[] { "SubscriptionVmId" });
            DropIndex("dbo.UserVms", new[] { "VmWareCenterId" });
            DropIndex("dbo.UserVms", new[] { "HyperVHostId" });
            DropIndex("dbo.UserVms", new[] { "UserId" });
            DropIndex("dbo.UserVms", new[] { "ServerTemplateId" });
            DropIndex("dbo.UserInfoes", new[] { "UserId" });
            DropIndex("dbo.OperatingSystems", new[] { "ImageFileId" });
            DropIndex("dbo.ServerTemplates", new[] { "UserId" });
            DropIndex("dbo.ServerTemplates", new[] { "OperatingSystemId" });
            DropIndex("dbo.ServerTemplates", new[] { "ImageFileId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.HelpDeskRequests", new[] { "UserId" });
            DropIndex("dbo.HelpDeskRequestComments", new[] { "RequestId" });
            DropIndex("dbo.HelpDeskRequestComments", new[] { "UserId" });
            DropIndex("dbo.Payments", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.SubscriptionVms", new[] { "TariffId" });
            DropIndex("dbo.BillingTransactions", new[] { "BillingTransactionId" });
            DropIndex("dbo.BillingTransactions", new[] { "UserId" });
            DropTable("dbo.FileDescriptorHelpDeskRequests");
            DropTable("dbo.TaskV2");
            DropTable("dbo.Statistics");
            DropTable("dbo.StateMachines");
            DropTable("dbo.SnapshotVms");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Regions");
            DropTable("dbo.PhoneCallRequests");
            DropTable("dbo.OAuthRefreshTokens");
            DropTable("dbo.OAuthClientApplications");
            DropTable("dbo.Messages");
            DropTable("dbo.LogEntries");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailInfoes");
            DropTable("dbo.Discounts");
            DropTable("dbo.VmWareVCenters");
            DropTable("dbo.SystemCenterVirtualManagers");
            DropTable("dbo.HyperVHostResources");
            DropTable("dbo.HyperVHosts");
            DropTable("dbo.UserVms");
            DropTable("dbo.UserInfoes");
            DropTable("dbo.OperatingSystems");
            DropTable("dbo.ServerTemplates");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.FileDescriptors");
            DropTable("dbo.HelpDeskRequests");
            DropTable("dbo.HelpDeskRequestComments");
            DropTable("dbo.Payments");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Tariffs");
            DropTable("dbo.SubscriptionVms");
            DropTable("dbo.BillingTransactions");
        }
    }
}

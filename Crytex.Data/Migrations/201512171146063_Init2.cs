namespace Crytex.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init2 : DbMigration
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
                        AdminUserId = c.String(maxLength: 128),
                        SubscriptionVmId = c.Guid(),
                        SubscriptionVmMonthCount = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.SubscriptionVms", t => t.SubscriptionVmId)
                .ForeignKey("dbo.AspNetUsers", t => t.AdminUserId)
                .Index(t => t.UserId)
                .Index(t => t.AdminUserId)
                .Index(t => t.SubscriptionVmId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Lastname = c.String(),
                        Patronymic = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        Address = c.String(),
                        CodePhrase = c.String(),
                        UserType = c.Int(nullable: false),
                        RegisterDate = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        IsBlocked = c.Boolean(nullable: false),
                        Payer = c.String(),
                        ContactPerson = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
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
                        CoreCount = c.Int(nullable: false),
                        RamCount = c.Int(nullable: false),
                        HardDriveSize = c.Int(nullable: false),
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
                        MinCoreCount = c.Int(nullable: false),
                        MinHardDriveSize = c.Int(nullable: false),
                        MinRamCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileDescriptors", t => t.ImageFileId, cascadeDelete: false)
                .Index(t => t.ImageFileId);
            
            CreateTable(
                "dbo.UserVms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        CoreCount = c.Int(nullable: false),
                        RamCount = c.Int(nullable: false),
                        HardDriveSize = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        OperatingSystemId = c.Int(nullable: false),
                        Name = c.String(),
                        UserId = c.String(maxLength: 128),
                        VirtualizationType = c.Int(nullable: false),
                        HyperVHostId = c.Guid(),
                        VmWareCenterId = c.Guid(),
                        OperatingSystemPassword = c.String(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.HyperVHosts", t => t.HyperVHostId)
                .ForeignKey("dbo.OperatingSystems", t => t.OperatingSystemId, cascadeDelete: true)
                .ForeignKey("dbo.VmWareVCenters", t => t.VmWareCenterId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.OperatingSystemId)
                .Index(t => t.UserId)
                .Index(t => t.HyperVHostId)
                .Index(t => t.VmWareCenterId);
            
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
                "dbo.VmIpAddresses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IPv4 = c.String(),
                        IPv6 = c.String(),
                        MAC = c.String(),
                        NetworkName = c.String(),
                        VmId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserVms", t => t.VmId, cascadeDelete: true)
                .Index(t => t.VmId);
            
            CreateTable(
                "dbo.SubscriptionVms",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        DateCreate = c.DateTime(nullable: false),
                        DateEnd = c.DateTime(nullable: false),
                        TariffId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        SubscriptionType = c.Int(nullable: false),
                        AutoDetection = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Tariffs", t => t.TariffId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.UserVms", t => t.Id)
                .Index(t => t.Id)
                .Index(t => t.TariffId)
                .Index(t => t.UserId);
            
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
                        Load10Percent = c.Double(nullable: false),
                        CreateDate = c.DateTime(nullable: false),
                        UpdateDate = c.DateTime(),
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
                "dbo.GameServerConfigurations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ServerTemplateId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ServerTemplates", t => t.ServerTemplateId)
                .Index(t => t.ServerTemplateId);
            
            CreateTable(
                "dbo.GameServers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PaymentType = c.Int(nullable: false),
                        VmId = c.Guid(nullable: false),
                        SlotCount = c.Int(nullable: false),
                        GameServerConfigurationId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GameServerConfigurations", t => t.GameServerConfigurationId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.UserVms", t => t.VmId, cascadeDelete: true)
                .Index(t => t.VmId)
                .Index(t => t.GameServerConfigurationId)
                .Index(t => t.UserId);
            
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
                "dbo.NetTrafficCounters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MachineId = c.Guid(nullable: false),
                        ReceiveKiloBytes = c.Long(nullable: false),
                        TransmittedKiloBytes = c.Long(nullable: false),
                        PeriodType = c.Int(nullable: false),
                        CountingPeriodStartDate = c.DateTime(nullable: false),
                        LastUpdateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserVms", t => t.MachineId, cascadeDelete: true)
                .Index(t => t.MachineId);
            
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
                "dbo.UserLoginLogEntries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(maxLength: 128),
                        LoginDate = c.DateTime(nullable: false),
                        IpAddress = c.String(),
                        WithDataSaving = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.VmBackups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        VmId = c.Guid(nullable: false),
                        Name = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserVms", t => t.VmId, cascadeDelete: true)
                .Index(t => t.VmId);
            
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
            DropForeignKey("dbo.VmBackups", "VmId", "dbo.UserVms");
            DropForeignKey("dbo.UserLoginLogEntries", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SnapshotVms", "VmId", "dbo.UserVms");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.NetTrafficCounters", "MachineId", "dbo.UserVms");
            DropForeignKey("dbo.LogEntries", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GameServers", "VmId", "dbo.UserVms");
            DropForeignKey("dbo.GameServers", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.GameServers", "GameServerConfigurationId", "dbo.GameServerConfigurations");
            DropForeignKey("dbo.GameServerConfigurations", "ServerTemplateId", "dbo.ServerTemplates");
            DropForeignKey("dbo.BillingTransactions", "AdminUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserVms", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserVms", "VmWareCenterId", "dbo.VmWareVCenters");
            DropForeignKey("dbo.SubscriptionVms", "Id", "dbo.UserVms");
            DropForeignKey("dbo.SubscriptionVms", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SubscriptionVms", "TariffId", "dbo.Tariffs");
            DropForeignKey("dbo.BillingTransactions", "SubscriptionVmId", "dbo.SubscriptionVms");
            DropForeignKey("dbo.UserVms", "OperatingSystemId", "dbo.OperatingSystems");
            DropForeignKey("dbo.VmIpAddresses", "VmId", "dbo.UserVms");
            DropForeignKey("dbo.UserVms", "HyperVHostId", "dbo.HyperVHosts");
            DropForeignKey("dbo.HyperVHosts", "SystemCenterVirtualManagerId", "dbo.SystemCenterVirtualManagers");
            DropForeignKey("dbo.HyperVHostResources", "HyperVHostId", "dbo.HyperVHosts");
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
            DropIndex("dbo.FileDescriptorHelpDeskRequests", new[] { "HelpDeskRequest_Id" });
            DropIndex("dbo.FileDescriptorHelpDeskRequests", new[] { "FileDescriptor_Id" });
            DropIndex("dbo.VmBackups", new[] { "VmId" });
            DropIndex("dbo.UserLoginLogEntries", new[] { "UserId" });
            DropIndex("dbo.SnapshotVms", new[] { "VmId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.NetTrafficCounters", new[] { "MachineId" });
            DropIndex("dbo.LogEntries", new[] { "UserId" });
            DropIndex("dbo.GameServers", new[] { "UserId" });
            DropIndex("dbo.GameServers", new[] { "GameServerConfigurationId" });
            DropIndex("dbo.GameServers", new[] { "VmId" });
            DropIndex("dbo.GameServerConfigurations", new[] { "ServerTemplateId" });
            DropIndex("dbo.SubscriptionVms", new[] { "UserId" });
            DropIndex("dbo.SubscriptionVms", new[] { "TariffId" });
            DropIndex("dbo.SubscriptionVms", new[] { "Id" });
            DropIndex("dbo.VmIpAddresses", new[] { "VmId" });
            DropIndex("dbo.HyperVHostResources", new[] { "HyperVHostId" });
            DropIndex("dbo.HyperVHosts", new[] { "SystemCenterVirtualManagerId" });
            DropIndex("dbo.UserVms", new[] { "VmWareCenterId" });
            DropIndex("dbo.UserVms", new[] { "HyperVHostId" });
            DropIndex("dbo.UserVms", new[] { "UserId" });
            DropIndex("dbo.UserVms", new[] { "OperatingSystemId" });
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
            DropIndex("dbo.BillingTransactions", new[] { "SubscriptionVmId" });
            DropIndex("dbo.BillingTransactions", new[] { "AdminUserId" });
            DropIndex("dbo.BillingTransactions", new[] { "UserId" });
            DropTable("dbo.FileDescriptorHelpDeskRequests");
            DropTable("dbo.VmBackups");
            DropTable("dbo.UserLoginLogEntries");
            DropTable("dbo.TaskV2");
            DropTable("dbo.Statistics");
            DropTable("dbo.StateMachines");
            DropTable("dbo.SnapshotVms");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Regions");
            DropTable("dbo.PhoneCallRequests");
            DropTable("dbo.OAuthRefreshTokens");
            DropTable("dbo.OAuthClientApplications");
            DropTable("dbo.NetTrafficCounters");
            DropTable("dbo.Messages");
            DropTable("dbo.LogEntries");
            DropTable("dbo.GameServers");
            DropTable("dbo.GameServerConfigurations");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailInfoes");
            DropTable("dbo.Discounts");
            DropTable("dbo.VmWareVCenters");
            DropTable("dbo.Tariffs");
            DropTable("dbo.SubscriptionVms");
            DropTable("dbo.VmIpAddresses");
            DropTable("dbo.SystemCenterVirtualManagers");
            DropTable("dbo.HyperVHostResources");
            DropTable("dbo.HyperVHosts");
            DropTable("dbo.UserVms");
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
            DropTable("dbo.BillingTransactions");
        }
    }
}

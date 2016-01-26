using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity.EntityFramework;
using Crytex.Model.Models;
using Crytex.Model.Models.Biling;

namespace Crytex.Data
{
    using Crytex.Model.Models.Notifications;
    using Model.Models.WebHosting;

    [DbConfigurationType(typeof(DbConfig))] 
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public virtual void Commit()
        {
            SaveChanges();
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
         
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            
            modelBuilder.Entity<OperatingSystem>()
                .HasRequired(t => t.ImageFileDescriptor).WithMany().HasForeignKey(system => system.ImageFileId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GameServerConfiguration>()
              .HasRequired(t => t.ServerTemplate).WithMany().HasForeignKey(system => system.ServerTemplateId)
              .WillCascadeOnDelete(false);
            modelBuilder.Entity<UsageSubscriptionPayment>()
                 .HasRequired(t => t.SubscriptionVm).WithMany().HasForeignKey(t => t.SubscriptionVmId)
                 .WillCascadeOnDelete(false);
            modelBuilder.Entity<FixedSubscriptionPayment>()
                 .HasRequired(t => t.SubscriptionVm).WithMany().HasForeignKey(t => t.SubscriptionVmId)
                 .WillCascadeOnDelete(false);
            modelBuilder.Entity<SubscriptionVmBackupPayment>()
                 .HasRequired(t => t.SubscriptionVm).WithMany().HasForeignKey(t => t.SubscriptionVmId)
                 .WillCascadeOnDelete(false);
            modelBuilder.Entity<WebHostingFtpAccount>()
                 .HasRequired(t => t.WebAppliaction).WithMany().HasForeignKey(t => t.WebApplicationId)
                 .WillCascadeOnDelete(false);
        }
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Tariff> Tariffs { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<HelpDeskRequest> HelpDeskRequests { get; set; }
        public DbSet<HelpDeskRequestComment> HelpDeskRequestComments { get; set; }
        public DbSet<StateMachine> StateMachines { get; set; }
        public DbSet<TaskV2> TaskV2 { get; set; }
        public DbSet<UserVm> UserVms { get; set; }
        public DbSet<FileDescriptor> Files { get; set; }
        public DbSet<OperatingSystem> OperatingSystems { get; set; }
        public DbSet<ServerTemplate> ServerTemplates { get; set; }
        public DbSet<BillingTransaction> BillingTransactions { get; set; }
        public DbSet<SubscriptionVm> SubscriptionVms { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<EmailInfo> EmailInfos { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        public DbSet<SystemCenterVirtualManager> SystemCenterVirtualManagers { get; set; }
        public DbSet<HyperVHost> HyperVHosts { get; set; }
        public DbSet<HyperVHostResource> HyperVHostResources { get; set; }
        public DbSet<SnapshotVm> SnapshotVm { get; set; }
        public DbSet<VmWareVCenter> VmWareVCenters { get; set; }
        public DbSet<OAuthClientApplication> OAuthClientApplications { get; set; }
        public DbSet<OAuthRefreshToken> OAuthRefreshTokens { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<PhoneCallRequest> PhoneCallRequests { get; set; }     
        public DbSet<NetTrafficCounter> NetTrafficCounters { get; set; }
        public DbSet<VmBackup> VmBackups { get; set; }
        public DbSet<UserLoginLogEntry> UserLoginLogEntries { get; set; }
		public DbSet<Trigger> Triggers { get; set; }
        public DbSet<GameServer> GameServers { get; set; }
        public DbSet<GameServerConfiguration> GameServerConfigurations { get; set; }
        public DbSet<VmIpAddress> VmIpAddresses { get; set; }
        public DbSet<UsageSubscriptionPayment> UsageSubscriptionPayments { get; set; }
        public DbSet<FixedSubscriptionPayment> FixedSubscriptionPayments { get; set; }
        public DbSet<SubscriptionVmBackupPayment> SubscriptionVmBackupPayments { get; set; }
        public DbSet<News> Newses { get; set; }
        public DbSet<PaymentGameServer> PaymentGameServers { get; set; }
        public DbSet<WebDomain> WebDomains { get; set; }
        public DbSet<WebDatabase> WebDatabases { get; set; }
        public DbSet<WebHostingFtpAccount> WebHostingFtpAccounts { get; set; }
        public DbSet<WebHostingTariff> WebHostingTariffs { get; set; }
        public DbSet<WebHosting> WebHostings { get; set; }
        public DbSet<HostedWebApplication> HostedWebApplications { get; set; }
        public DbSet<WebDatabaseServer> WebDatabaseServers { get; set; }
        public DbSet<WebHttpServer> WebHttpServers { get; set; }
    }
}

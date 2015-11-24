using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Crytex.Model.Models;

namespace Crytex.Data
{
    using Crytex.Model.Models.Notifications;

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
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<BillingTransaction> BillingTransactions { get; set; }
        public DbSet<CreditPaymentOrder> CreditPaymentOrders { get; set; }
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
    }
}

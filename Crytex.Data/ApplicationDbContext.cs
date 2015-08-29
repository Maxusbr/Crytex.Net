using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Crytex.Data.Repository;
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


        public DbSet<Message> Messages { get; set; }

        public DbSet<HelpDeskRequest> HelpDeskRequests { get; set; }

        public DbSet<CreateVmTask> CreateVmTasks { get; set; }
        public DbSet<StandartVmTask> StandartVmTasks { get; set; }
        public DbSet<UpdateVmTask> UpdateVmTasks { get; set; }
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
    }
}

using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Data.Repository;
using Project.Model.Models;

namespace Project.Data
{
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
    }
}

using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Project.Model.Models;

namespace Project.Data
{
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

        public DbSet<RemoveVmTask> RemoveVmTasks { get; set; }
        public DbSet<StandartVmTask> StandartVmTasks { get; set; }
        public DbSet<CreateVm> UpdateVmTasks { get; set; }
        public DbSet<UserVm> UserVms { get; set; }
    }
}

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
    }
}

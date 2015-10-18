using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Crytex.Model.Models;
using Crytex.Model.Enums;

namespace Crytex.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            if (!context.OAuthClientApplications.Any())
            {
                context.OAuthClientApplications.Add(new OAuthClientApplication
                {
                    Id = "CrytexAngularApp",
                    Secret = Crytex.Model.Helpers.Helper.GetHash("abc@123_ololo"),
                    Name = "Crytex Front-End Angualar Based SPA",
                    EnumApplicationType = EnumApplicationType.JavaScript,
                    Active = true,
                    RefreshTokenLifeTime = 7200
                    //AllowedOrigin = "*"
                });
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var user = new ApplicationUser()
            {
                UserName = "AdminUser",
                Email = "admin@admin.com",
                EmailConfirmed = true,
            };

            manager.Create(user, "wUcheva$3a");

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "Support" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var adminUser = manager.FindByName("AdminUser");
            manager.AddToRoles(adminUser.Id, new string[] { "Admin"});

            var defVCenter = new VmWareVCenter
            {
                Name = "default",
                UserName = "administrator@vsphere.local",
                Password = "QwerT@12",
                ServerAddress = "51.254.55.136"
            };
            context.VmWareVCenters.Add(defVCenter);
            context.Commit();
        }
    }
}

using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Crytex.Core;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Crytex.Model.Models;
using Crytex.Model.Enums;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;
using System.IO;

namespace Crytex.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        readonly private bool CreateFakeEntries;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            CreateFakeEntries = true;
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

            var defVCenter = new VmWareVCenter
            {
                Name = "default",
                UserName = "administrator@vsphere.local",
                Password = "QwerT@12",
                ServerAddress = "51.254.55.136"
            };
            if (context.VmWareVCenters.All(c => c.Name != defVCenter.Name))
                context.VmWareVCenters.Add(defVCenter);


            if (this.CreateFakeEntries)
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                if (!roleManager.Roles.Any())
                {
                    roleManager.Create(new IdentityRole { Name = "Admin" });
                    roleManager.Create(new IdentityRole { Name = "Support" });
                    roleManager.Create(new IdentityRole { Name = "User" });
                }

                var allUsers = context.Users.ToList();
                for (int i = 1; i < 3; i++)
                {
                    var admin = new ApplicationUser()
                    {
                        UserName = "AdminUser" + i,
                        Email = "admin" + i + "@admin.com",
                        EmailConfirmed = true,
                    };
                    if (allUsers.All(u => u.UserName != admin.UserName))
                    {
                        manager.Create(admin, "wUcheva$3a");
                        manager.AddToRoles(admin.Id, new string[] { "Admin" });
                    }
                }

                for (int i = 1; i < 4; i++)
                {
                    var support = new ApplicationUser()
                    {
                        UserName = "SupportUser" + i,
                        Email = "support" + i + "@admin.com",
                        EmailConfirmed = true,
                    };
                    if (allUsers.All(u => u.UserName != support.UserName))
                    {
                        manager.Create(support, "wUcheva$3a");
                        manager.AddToRoles(support.Id, new string[] { "Support" });
                    }
                }

                for (int i = 1; i < 6; i++)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = "User" + i,
                        Email = "user" + i + "@admin.com",
                        EmailConfirmed = true,
                    };
                    if (allUsers.All(u => u.UserName != user.UserName))
                    {
                        manager.Create(user, "wUcheva$3a");
                        manager.AddToRoles(user.Id, new string[] { "User" });
                    }
                }

                // ///////////////////////////////////

                for (int i = 1; i < 6; i++)
                {
                    var userRequest = manager.FindByName("User" + i);
                    var adminRequest = manager.FindByName("AdminUser" + 1);
                    var helpDeskRequest = new HelpDeskRequest
                    {
                        UserId = userRequest.Id,
                        Summary = "Summary",
                        Details = "Details",
                        Status = RequestStatus.New,
                        CreationDate = this.RandomDay(),
                        Read = true,
                        Id = 100000 + i
                    };

                    context.HelpDeskRequests.Add(helpDeskRequest);

                    for (int b = 1; b < 3; b++)
                    {
                        var commentUser = new HelpDeskRequestComment
                        {
                            Comment = "CommentUser #" + b,
                            CreationDate = this.RandomDay(),
                            UserId = userRequest.Id,
                            RequestId = helpDeskRequest.Id,
                            Id = 100000 + b
                        };

                        context.HelpDeskRequestComments.Add(commentUser);
                    }

                    for (int k = 1; k < 3; k++)
                    {
                        var commentAdmin = new HelpDeskRequestComment
                        {
                            Comment = "CommentAdmin #" + k,
                            CreationDate = RandomDay(),
                            UserId = adminRequest.Id,
                            RequestId = helpDeskRequest.Id,
                            Id = 100100 + k
                        };

                        context.HelpDeskRequestComments.Add(commentAdmin);
                    }

                }

                for (int i = 1; i < 6; i++)
                {
                    var userRequest = manager.FindByName("User" + i);
                    var helpDeskRequest = new HelpDeskRequest
                    {
                        UserId = userRequest.Id,
                        Summary = "Summary",
                        Details = "Details",
                        Status = RequestStatus.New,
                        CreationDate = RandomDay(),
                        Read = false,
                        Id = 100100 + i
                    };

                    context.HelpDeskRequests.Add(helpDeskRequest);
                }


                var userForLog = manager.FindByName("User" + 1);

                for (int i = 0; i < 80; i++)
                {
                    string source = null;
                    if (i < 20) source = SourceLog.Web.ToString("G");
                    if (i > 20) source = SourceLog.API.ToString("G");
                    if (i > 40) source = SourceLog.Background.ToString("G");
                    if (i > 60) source = SourceLog.ExecutorTask.ToString("G");

                    var log = new LogEntry
                    {
                        UserId = userForLog.Id,
                        Date = this.RandomDay(),
                        Message = "Message " + i,
                        Source = source
                    };
                    context.LogEntries.Add(log);
                }
                ////////////////////////////////////////////////
                var path = this.CreateImage();
                FileDescriptor image;
                //"/imageSavePath"
                if (context.Files.All(f => f.Path != path))
                {
                    image = new FileDescriptor
                    {
                        Name = "ImageTest",
                        Path = path,
                        Type = FileType.Image,
                        Id = 100000
                    };
                    context.Files.Add(image);
                }
                else
                {
                    image = context.Files.First(i => i.Path == path);
                }

                var allOperations = context.OperatingSystems.ToList();
                OperatingSystem[] operations = new OperatingSystem[2];
                for (int i = 0; i < 2; i++)
                {
                    operations[i] = new OperatingSystem
                    {
                        Name = "OperationgSystem" + i,
                        Description = "Description",
                        ImageFileId = image.Id,
                        ServerTemplateName = "ServerTemplateName",
                    };
                    if (allOperations.All(o => o.Name != operations[i].Name))
                        context.OperatingSystems.Add(operations[i]);
                    else
                        operations[i] = allOperations.First(o => o.Name == operations[i].Name);
                }


                for (int i = 0; i < 2; i++)
                {
                    var serverTemplate = new ServerTemplate
                    {
                        Name = "Name" + i,
                        Description = "Description",
                        MinCoreCount = 20,
                        MinRamCount = 300,
                        MinHardDriveSize = 4000,
                        ImageFileId = image.Id,
                        OperatingSystemId = operations[0].Id,
                        UserId = userForLog.Id
                    };
                    context.ServerTemplates.Add(serverTemplate);
                }

                for (int i = 0; i < 2; i++)
                {
                    var serverTemplate = new ServerTemplate
                    {
                        Name = "Name" + i,
                        Description = "Description",
                        MinCoreCount = 20,
                        MinRamCount = 300,
                        MinHardDriveSize = 4000,
                        ImageFileId = image.Id,
                        OperatingSystemId = operations[1].Id,
                        UserId = userForLog.Id
                    };
                    context.ServerTemplates.Add(serverTemplate);
                }
            }

            context.Commit();
        }

        private DateTime RandomDay()
        {
            DateTime start = new DateTime(1995, 1, 1);
            Random gen = new Random();

            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }

        private string CreateImage()
        {
            var rootFolder = Directory.GetParent(@"../").FullName;
            
            string newFilePath = rootFolder + @"\Crytex.Web\App_Data\Files\Images";
            Directory.CreateDirectory(newFilePath);

            string nameFile = "ImageTest.jpg";
            newFilePath += @"\small_" + nameFile;
            
            string currentFilePath = rootFolder + @"\" + nameFile;
            File.Copy(currentFilePath, newFilePath, true);

            return nameFile;
        }
    }
}

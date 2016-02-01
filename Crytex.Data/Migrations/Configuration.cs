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
using Crytex.Model.Models.Biling;
using Crytex.Model.Models.Notifications;

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
        protected  override void Seed(ApplicationDbContext context)
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

            if (!context.SystemCenterVirtualManagers.Any())
            {
                var systemCenter = new SystemCenterVirtualManager()
                {

                    UserName = "username",
                    Password = "password",
                    Host = "51.254.55.136",

                };
                context.SystemCenterVirtualManagers.Add(systemCenter);
                context.Commit();
                var hyperVDev = new HyperVHost()
                {
                    DateAdded = DateTime.UtcNow,
                    UserName = "username",
                    Password = "password",
                    Host = "51.254.55.136",
                    SystemCenterVirtualManagerId = systemCenter.Id
                };
                context.HyperVHosts.Add(hyperVDev);
                context.Commit();


            }
            if (this.CreateFakeEntries)
            {
                Random random = new Random();
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
                        RegisterDate = DateTime.Now,
                        Country = "Россия",
                        ContactPerson = "Контактное лицо",
                        Payer = "Плательщик"
                    };
                    if (allUsers.All(u => u.UserName != admin.UserName))
                    {
                        manager.Create(admin, "wUcheva$3a");
                        manager.AddToRoles(admin.Id, new string[] { "Admin" });
                        allUsers.Add(admin);
                    }
                }

                for (int i = 1; i < 4; i++)
                {
                    var support = new ApplicationUser()
                    {
                        UserName = "SupportUser" + i,
                        Email = "support" + i + "@admin.com",
                        EmailConfirmed = true,
                        RegisterDate = DateTime.Now,
                        Country = "Россия",
                        ContactPerson = "Контактное лицо",
                        Payer = "Плательщик"
                    };
                    if (allUsers.All(u => u.UserName != support.UserName))
                    {
                        manager.Create(support, "wUcheva$3a");
                        manager.AddToRoles(support.Id, new string[] { "Support" });
                        allUsers.Add(support);
                    }
                }

                for (int i = 1; i < 6; i++)
                {
                    var user = new ApplicationUser()
                    {
                        UserName = "User" + i,
                        Email = "user" + i + "@admin.com",
                        EmailConfirmed = true,
                        RegisterDate = DateTime.Now,
                        Country = "Россия",
                        ContactPerson = "Контактное лицо",
                        Payer = "Плательщик"
                    };
                    if (allUsers.All(u => u.UserName != user.UserName))
                    {
                        manager.Create(user, "wUcheva$3a");
                        manager.AddToRoles(user.Id, new string[] { "User" });
                        allUsers.Add(user);
                    }
                }

                ///////////////////////////////////
                var regApproveEmailtemplate = new EmailTemplate
                {
                    Body = @"Подтвердите вашу учетную запись, щелкнув <a href=""{callbackUrl}"">здесь</a>",
                    EmailTemplateType = EmailTemplateType.Registration,
                    ParameterNames = @"[""callbackUrl""]",
                    Subject = "Подтверждение учетной записи"
                };
                var subscriptionNeedsPaymentEmailTemplate = new EmailTemplate
                {
                    Subject = "Подписка требует оплаты",
                    EmailTemplateType = EmailTemplateType.SubscriptionNeedsPayment,
                    Body = "Ваша подписка на виртуальную машину требует оплаты. Пожалуйста внесите платёж. Машина отключена"
                };
                var subscriptionEndWarningEmailTemplate = new EmailTemplate
                {
                    Subject = "Срок подписки истекает",
                    EmailTemplateType = EmailTemplateType.SubscriptionEndWarning,
                    Body = "Ваша подписка на виртуальную машину истекает через {daysToEnd} дня",
                    ParameterNames = @"[""daysToEnd""]"
                };
                var subscriptionDeletionWarningEmailTemplate = new EmailTemplate
                {
                    Subject = "Ваша подписка будет удалена",
                    EmailTemplateType = EmailTemplateType.SubscriptionDeletionWarning,
                    Body = "Ваша подписка на виртуальную машину буден удаоена через {daysToDeletion} дня",
                    ParameterNames = @"[""daysToDeletion""]"
                };
                var createVmCredsEmailTemplate = new EmailTemplate
                {
                    Subject = "Ваша машина была создана",
                    EmailTemplateType = EmailTemplateType.CreateVmCredentials,
                    Body = "Ваша машина {vmName} создана для доступа используйте имя пользователя {osUserName} и пароль {osUserPassword}",
                    ParameterNames = @"[""vmName"", ""osUserName"", ""osUserPassword""]"
                };

                context.EmailTemplates.Add(regApproveEmailtemplate);
                context.EmailTemplates.Add(subscriptionNeedsPaymentEmailTemplate);
                context.EmailTemplates.Add(subscriptionEndWarningEmailTemplate);
                context.EmailTemplates.Add(subscriptionDeletionWarningEmailTemplate);
                context.EmailTemplates.Add(createVmCredsEmailTemplate);
                ///////////////////////////////////

                for (int i = 1; i < 6; i++)
                {
                    var userRequest = manager.FindByName("User" + i);
                    var adminRequest = manager.FindByName("AdminUser" + 1);
                    var helpDeskRequest = new HelpDeskRequest
                    {
                        UserId = userRequest.Id,
                        Summary = "Заголовок",
                        Details = "Описание описание описание проблемы",
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
                            Comment = "Коментарий #" + b,
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
                            Comment = "Комментарий #" + k,
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
                        Summary = "Заголовок",
                        Details = "Описание описание описание проблемы",
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
                        Name = "Операционная системы " + i,
                        Description = "Описание ",
                        ImageFileId = image.Id,
                        ServerTemplateName = "ServerTemplateName",
                        Family = (OperatingSystemFamily)i,
                        DefaultAdminPassword = "Password123",
                        DefaultAdminName = "Administrator",
                        MinCoreCount = 2,
                        MinHardDriveSize = 50000,
                        MinRamCount = 512
                    };
                    if (allOperations.All(o => o.Name != operations[i].Name))
                        context.OperatingSystems.Add(operations[i]);
                    else
                        operations[i] = allOperations.First(o => o.Name == operations[i].Name);
                }

                context.Commit();

                ServerTemplate[] serverTemplates = new ServerTemplate[4];
                var allTemplates = context.ServerTemplates.ToList();
                for (int i = 0; i < 4; i++)
                {
                    var name = (i < 2) ? "Шаблон Window" : "Шаблон Ubuntu";
                    var target = (i < 2) ? 0 : 1;

                    serverTemplates[i] = new ServerTemplate
                    {
                        Name = name + i,
                        Description = "Description",
                        CoreCount = 2,
                        RamCount = 512,
                        HardDriveSize = 500,
                        ImageFileId = image.Id,
                        OperatingSystemId = operations[target].Id,
                    };

                    if (allTemplates.All(o => o.Name != serverTemplates[i].Name))
                        context.ServerTemplates.Add(serverTemplates[i]);
                    else
                        serverTemplates[i] = allTemplates.First(o => o.Name == serverTemplates[i].Name);
                }
                context.Commit();

                var allMachine = context.UserVms.ToList();
                for (int i = 1; i < 3; i++)
                {
                    var adminUser = allUsers.First(u => u.UserName == "AdminUser" + i);
                    for (int c = 0; c < 4; c++)
                    {
                        var vm = new UserVm
                        {
                            Id = Guid.NewGuid(),
                            HardDriveSize = 50000,
                            CoreCount = 2,
                            RamCount = 512,
                            Status = StatusVM.Disable,
                            UserId = adminUser.Id,
                            OperatingSystemId = operations[random.Next(operations.Length)].Id,
                            Name = "Machine " + i + "" + c,
                            VirtualizationType = (c < 2) ? TypeVirtualization.HyperV : TypeVirtualization.VmWare,
                            OperatingSystemPassword = "1111",
                            CreateDate = DateTime.UtcNow,
                        };

                        if (c < 2) vm.HyperVHostId = context.HyperVHosts.First().Id;
                        else if (c >= 2) vm.VmWareCenterId = context.VmWareVCenters.First().Id;

                        if (allMachine.All(o => o.Name != vm.Name))
                            context.UserVms.Add(vm);
                    }
                }

                for (int i = 1; i < 5; i++)
                {
                    var userMoney = allUsers.First(u => u.UserName == "User" + i);
                    var valuesTransaction = Enum.GetValues(typeof(BillingTransactionType));
                    var valuesPayment = Enum.GetValues(typeof(PaymentSystemType));

                    for (int b = 0; b < 2; b++)
                    {
                        foreach (var typeTrans in valuesTransaction)
                        {
                            var cash = random.Next(200, 1000);
                            var transaction = new BillingTransaction
                            {
                                TransactionType = (BillingTransactionType)typeTrans,
                                Date = RandomDateCurrentMonth(DateTime.UtcNow),
                                CashAmount = cash,
                                Description = "description transaction on" + cash + "$",
                                UserId = userMoney.Id,
                            };
                            context.BillingTransactions.Add(transaction);
                        }
                    }

                    for (int b = 0; b < 2; b++)
                    {
                        foreach (var typePayment in valuesPayment)
                        {
                            var cash = random.Next(200, 1000);
                            var startDate = RandomDateCurrentMonth(DateTime.UtcNow);
                            var payment = new Payment
                            {
                                CashAmount = cash,
                                Date = startDate,
                                DateEnd = startDate.AddHours(1),
                                UserId = userMoney.Id,
                                Success = (random.Next(2) > 0),
                                PaymentSystem = (PaymentSystemType)typePayment
                            };
                            context.Payments.Add(payment);
                        }
                    }
                }
                for (int i = 0; i < 10; i++)
                {
                    var call = new PhoneCallRequest
                    {
                        PhoneNumber = "38067777827" + i,
                        CreationDate = DateTime.UtcNow,
                        IsRead = (random.Next(2) > 0)
                    };
                    context.PhoneCallRequests.Add(call);
                }

                var allTarifs = context.Tariffs.ToList();
                for (int i = 0; i < 4; i++)
                {
                    var tariff = new Tariff
                    {
                        Virtualization = (i < 2) ? TypeVirtualization.HyperV : TypeVirtualization.VmWare,
                        OperatingSystem = (i == 0 || i == 2) ? OperatingSystemFamily.Ubuntu : OperatingSystemFamily.Windows2012,
                        Processor1 = 10,
                        RAM512 = 2048,
                        HDD1 = 300,
                        SSD1 = 300,
                        Load10Percent = 1,
                        CreateDate = DateTime.UtcNow
                    };
                    
                    if (allTarifs.All(t => t.Virtualization != tariff.Virtualization && t.OperatingSystem != tariff.OperatingSystem))
                        context.Tariffs.Add(tariff);
                }
                context.Commit();

                for (int i = 0; i < 10; i++)
                {
                    var iterUser = (i + 1 < 6) ? i + 1 : 5;
                    var createVmOptions = new CreateVmOptions
                    {
                        Cpu = 2,
                        Hdd = 300,
                        Ram = 2048,
                        OperatingSystemId = operations[0].Id,
                        Name = "Машина #" + i,
                        UserVmId = Guid.NewGuid()
                    };

                    var user = allUsers.First(u => u.UserName == "User" + iterUser);

                    var createTask = new TaskV2
                    {
                        TypeTask = TypeTask.CreateVm,
                        Virtualization = (i < 5) ? TypeVirtualization.HyperV : TypeVirtualization.VmWare,
                        UserId = user.Id,
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow,
                        StatusTask = StatusTask.Pending,
                    };
                    createTask.SaveOptions<CreateVmOptions>(createVmOptions);

                    context.TaskV2.Add(createTask);

                    var newVm = new UserVm
                    {
                        Id = createVmOptions.UserVmId,
                        CoreCount = createVmOptions.Cpu,
                        HardDriveSize = createVmOptions.Hdd,
                        RamCount = createVmOptions.Ram,
                        VirtualizationType = createTask.Virtualization,
                        Name = createVmOptions.Name,
                        OperatingSystemId = createVmOptions.OperatingSystemId,
                        Status = StatusVM.Creating,
                        UserId = createTask.UserId
                    };

                    context.UserVms.Add(newVm);

                    var tariff = context.Tariffs.First(t => t.Virtualization == createTask.Virtualization && t.OperatingSystem == OperatingSystemFamily.Windows2012);

                    // Create new subscription
                    var subscritionDateEnd = DateTime.UtcNow.AddMonths(5);
                    var date = DateTime.Now;

                    var newSubscription = new SubscriptionVm
                    {
                        Id = createVmOptions.UserVmId,
                        AutoProlongation = (i < 5),
                        DateCreate = DateTime.UtcNow,
                        DateEnd = subscritionDateEnd,
                        UserId = user.Id,
                        Name = createVmOptions.Name,
                        SubscriptionType = (i < 5) ? SubscriptionType.Fixed : SubscriptionType.Usage,
                        TariffId = tariff.Id,
                        Status = SubscriptionVmStatus.Active,
                        LastUsageBillingTransactionDate = subscritionDateEnd,
                        UserVm = newVm
                    };
                    context.SubscriptionVms.Add(newSubscription);

                }

                context.Commit();

                for (int i = 0; i < 5; i++)
                {
                    var emailInfo = new EmailInfo
                    {
                        DateSending = DateTime.UtcNow,
                        From = "test@email.com",
                        To = "Crytex",
                        IsProcessed = true,
                        Reason = "Test",
                        SubjectParams = "",
                        BodyParams = "",
                        EmailTemplateType = EmailTemplateType.Registration,
                        EmailResultStatus = EmailResultStatus.Sent
                    };

                    context.EmailInfos.Add(emailInfo);
                }

                var template = serverTemplates[0].Id;
                var gameServerConfig = context.GameServerConfigurations.FirstOrDefault(
                    g => g.ServerTemplateId == template);

                if (gameServerConfig == null)
                {
                    gameServerConfig = new GameServerConfiguration
                    {
                        ServerTemplateId = serverTemplates[0].Id,
                    };
                    context.GameServerConfigurations.Add(gameServerConfig);
                }


                var gameServer = new GameServer
                {
                    PaymentType = ServerPaymentType.Slot,
                    VmId = allMachine[0].Id,
                    SlotCount = 5,
                    GameServerConfigurationId = gameServerConfig.Id,
                    UserId = allUsers[0].Id
                };
                context.GameServers.Add(gameServer);


                var fixedSubscriptions =
                    context.SubscriptionVms.Where(s => s.SubscriptionType == SubscriptionType.Fixed).ToList();
                foreach (var fixedSub in fixedSubscriptions)
                {
                    var adminUser = allUsers.First(u => u.UserName == "AdminUser" + 1);
                    var transaction = new BillingTransaction
                    {
                        Id = Guid.NewGuid(),
                        CashAmount = -2000,
                        TransactionType = BillingTransactionType.OneTimeDebiting,
                        SubscriptionVmMonthCount = 10,
                        UserId = fixedSub.UserId,
                        AdminUserId = adminUser.Id,
                        SubscriptionVmId = fixedSub.Id,
                        Description = "test",
                        Date = DateTime.UtcNow
                    };

                    var fixedPaymentSubscription = new FixedSubscriptionPayment
                    {
                        Date = DateTime.UtcNow,
                        DateStart = DateTime.UtcNow,
                        DateEnd = DateTime.UtcNow.AddHours(5),
                        Amount = 2000,
                        MonthCount = 10,
                        CoreCount = 2,
                        RamCount = 2048,
                        HardDriveSize = 300,
                        TariffId = allTarifs[0].Id,
                        BillingTransactionId = transaction.Id,
                        SubscriptionVmId = fixedSub.Id,
                    };
                    context.BillingTransactions.Add(transaction);
                    context.FixedSubscriptionPayments.Add(fixedPaymentSubscription);
                };

                var usageSubscriptions = context.SubscriptionVms.Where(s => s.SubscriptionType == SubscriptionType.Usage).ToList();
                foreach (var usageSub in usageSubscriptions)
                {
                    var adminUser = allUsers.First(u => u.UserName == "AdminUser" + 1);
                    var transaction = new BillingTransaction
                    {
                        Id = Guid.NewGuid(),
                        CashAmount = -2000,
                        TransactionType = BillingTransactionType.OneTimeDebiting,
                        UserId = usageSub.UserId,
                        AdminUserId = adminUser.Id,
                        SubscriptionVmId = usageSub.Id,
                        Description = "test",
                        Date = DateTime.UtcNow
                    };

                    var usagePaymentSubscription = new UsageSubscriptionPayment
                    {
                        Date = DateTime.UtcNow,
                        Amount = 2000,
                        Paid = false,
                        CoreCount = 2,
                        RamCount = 2048,
                        HardDriveSize = 300,
                        TariffId = allTarifs[0].Id,
                        BillingTransactionId = transaction.Id,
                        SubscriptionVmId = usageSub.Id,
                    };
                    context.BillingTransactions.Add(transaction);
                    context.UsageSubscriptionPayments.Add(usagePaymentSubscription);
                };
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

        private DateTime RandomDateCurrentMonth(DateTime dateNow)
        {
            Random gen = new Random();
            return new DateTime(dateNow.Year, dateNow.Month, gen.Next(1, 29));
        }

        private string CreateImage()
        {
            var rootFolder = Directory.GetParent(@"./").FullName;

            string newFilePath = rootFolder + @"\Crytex.Web\App_Data\Files\Images";
            Directory.CreateDirectory(newFilePath);

            string nameFile = "ImageTest.jpg";
            newFilePath += @"\small_" + nameFile;

            string currentFilePath = rootFolder + @"\" + nameFile;
            //File.Copy(currentFilePath, newFilePath, true);

            return nameFile;
        }
    }
}

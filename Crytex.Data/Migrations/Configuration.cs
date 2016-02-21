using System;
using System.Collections.Generic;
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
        readonly private bool CreateFakeEntriesEnabled;
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            CreateFakeEntriesEnabled = false;
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

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "Support" });
                roleManager.Create(new IdentityRole { Name = "FirstStepRegister" });
                roleManager.Create(new IdentityRole { Name = "User" });
            }

            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var admin = new ApplicationUser()
            {
                UserName = "AdminUser",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                RegisterDate = DateTime.Now,
                Country = "Россия",
                ContactPerson = "Контактное лицо",
                Payer = "Плательщик",
                PhoneNumberConfirmed = true
            };
            if(manager.Users.Where(user => user.UserName == "AdminUser").Any() == false)
            {
                manager.Create(admin, "adsfdg");
                manager.AddToRoles(admin.Id, new string[] { "Admin", "User" });
            }

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
                Body = "Ваша подписка на виртуальную машину будет удалена через {daysToDeletion} дня",
                ParameterNames = @"[""daysToDeletion""]"
            };
            var createVmCredsEmailTemplate = new EmailTemplate
            {
                Subject = "Ваша машина была создана",
                EmailTemplateType = EmailTemplateType.CreateVmCredentials,
                Body = "Ваша машина {vmName} создана для доступа используйте имя пользователя {osUserName} и пароль {osUserPassword}",
                ParameterNames = @"[""vmName"", ""osUserName"", ""osUserPassword""]"
            };
            var gameserverEndWarningEmailTemplate = new EmailTemplate
            {
                Subject = "Срок подписки истекает",
                EmailTemplateType = EmailTemplateType.GameServerEndWarning,
                Body = "Ваша подписка на виртуальную машину истекает через {daysToEnd} дня",
                ParameterNames = @"[""daysToEnd""]"
            };
            var gameserverNeedsPaymentEmailTemplate = new EmailTemplate
            {
                Subject = "Подписка требует оплаты",
                EmailTemplateType = EmailTemplateType.GameServerNeedsPayment,
                Body = "Ваша подписка на игровой сервер требует оплаты. Пожалуйста внесите платёж. Машина отключена"
            };
            var gameserverDeletionWarningEmailTemplate = new EmailTemplate
            {
                Subject = "Ваша подписка будет удалена",
                EmailTemplateType = EmailTemplateType.GameServerDeletionWarning,
                Body = "Ваша подписка на игровой сервер будет удалена через {daysToDeletion} дня",
                ParameterNames = @"[""daysToDeletion""]"
            };
            var resetPasswordTemplate = new EmailTemplate
            {
                Body = @"Для сброса пароля перейдите по ссылке <a href=""{callbackUrl}"">здесь</a>",
                EmailTemplateType = EmailTemplateType.ResetPassword,
                ParameterNames = @"[""callbackUrl""]",
                Subject = "Сброс пароля учетной записи"
            };

            var webHostingDisabledEmailTemplate = new EmailTemplate
            {
                Subject = "Веб-хостинг требует оплаты",
                EmailTemplateType = EmailTemplateType.WebHostingWasDisabled,
                Body = "Веб-хостинг требует оплаты. Пожалуйста внесите платёж. Хостинг временно отключен"
            };
            var webHostingEndWarningEmailTemplate = new EmailTemplate
            {
                Subject = "Срок оплаты веб-хостинга истекает",
                EmailTemplateType = EmailTemplateType.WebHostingEndWaring,
                Body = "Срок оплаты веб-хостинга истекает через {daysToEnd} дня",
                ParameterNames = @"[""daysToEnd""]"
            };
            var webHostingDeletionWarningEmailTemplate = new EmailTemplate
            {
                Subject = "Веб-хостинг будет удалён",
                EmailTemplateType = EmailTemplateType.WebHostingDeletionWarning,
                Body = "Ваш веб-хостинг будет удалена через {daysToDeletion} дня",
                ParameterNames = @"[""daysToDeletion""]"
            };

            var physicserverEndWarningEmailTemplate = new EmailTemplate
            {
                Subject = "Срок подписки истекает",
                EmailTemplateType = EmailTemplateType.PhysicalServerEndWarning,
                Body = "Ваша подписка на физический сервер истекает через {daysToEnd} дня",
                ParameterNames = @"[""daysToEnd""]"
            };
            var physicserverNeedsPaymentEmailTemplate = new EmailTemplate
            {
                Subject = "Подписка требует оплаты",
                EmailTemplateType = EmailTemplateType.PhysicalServerNeedsPayment,
                Body = "Ваша подписка на физический сервер требует оплаты. Пожалуйста внесите платёж. Машина отключена."
            };
            var physicserverDeletionWarningEmailTemplate = new EmailTemplate
            {
                Subject = "Ваша подписка будет удалена",
                EmailTemplateType = EmailTemplateType.PhysicalServerDeletionWarning,
                Body = "Ваша подписка на физический сервер будет удалена через {daysToDeletion} дня",
                ParameterNames = @"[""daysToDeletion""]"
            };


            var physicServerAdminCreated = new EmailTemplate
            {
                Subject = "Ваш физический сервер в процессе создания",
                EmailTemplateType = EmailTemplateType.PhysicalServerCreated,
                Body = "Ваш физический сервер находится в процессе создания и настройки.",
            };
            var physicServerAdminReady = new EmailTemplate
            {
                Subject = "Ваша физический сервер создан",
                EmailTemplateType = EmailTemplateType.PhysicalServerReady,
                Body = "Ваш физический сервер создан. {AdminMessage}",
                ParameterNames = @"[""AdminMessage""]"
            };
            var physicServerAdminDontCreate = new EmailTemplate
            {
                Subject = "Ваша физический сервер не может быть создан",
                EmailTemplateType = EmailTemplateType.PhysicalServerDontCreate,
                Body = "Ваш физический сервер не может быть создан. {AdminMessage}",
                ParameterNames = @"[""AdminMessage""]"
            };

            context.EmailTemplates.Add(regApproveEmailtemplate);
            context.EmailTemplates.Add(subscriptionNeedsPaymentEmailTemplate);
            context.EmailTemplates.Add(subscriptionEndWarningEmailTemplate);
            context.EmailTemplates.Add(subscriptionDeletionWarningEmailTemplate);
            context.EmailTemplates.Add(createVmCredsEmailTemplate);
            context.EmailTemplates.Add(gameserverEndWarningEmailTemplate);
            context.EmailTemplates.Add(gameserverNeedsPaymentEmailTemplate);
            context.EmailTemplates.Add(gameserverDeletionWarningEmailTemplate);
            context.EmailTemplates.Add(resetPasswordTemplate);
            context.EmailTemplates.Add(physicserverEndWarningEmailTemplate);
            context.EmailTemplates.Add(physicserverNeedsPaymentEmailTemplate);
            context.EmailTemplates.Add(physicserverDeletionWarningEmailTemplate);
            context.EmailTemplates.Add(physicServerAdminCreated);
            context.EmailTemplates.Add(physicServerAdminReady);
            context.EmailTemplates.Add(physicServerAdminDontCreate);
            context.EmailTemplates.Add(webHostingDisabledEmailTemplate);

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

            var onpay = new PaymentSystem { Name = "Onpay", IsEnabled = true };
            var sprypay = new PaymentSystem { Name = "Sprypay", IsEnabled = true };
            var interkassa = new PaymentSystem { Name = "Interkassa", IsEnabled = true };
            var payPal = new PaymentSystem { Name = "PayPal", IsEnabled = true };
            var webMoney = new PaymentSystem { Name = "WebMoney", IsEnabled = true };
            var yandexMoney = new PaymentSystem { Name = "YandexMoney", IsEnabled = true };
            context.PaymentSystems.Add(onpay);
            context.PaymentSystems.Add(sprypay);
            context.PaymentSystems.Add(interkassa);
            context.PaymentSystems.Add(payPal);
            context.PaymentSystems.Add(webMoney);
            context.PaymentSystems.Add(yandexMoney);
            context.Commit();

            if (this.CreateFakeEntriesEnabled)
            {
                this.CreateFakeEntries(context, manager);
            }
        }

        private void CreateFakeEntries(ApplicationDbContext context, UserManager<ApplicationUser> usrManager)
        {
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
            Random random = new Random();


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
                    Payer = "Плательщик",
                    PhoneNumberConfirmed = true

                };
                if (allUsers.All(u => u.UserName != admin.UserName))
                {
                    usrManager.Create(admin, "adsfdg");
                    usrManager.AddToRoles(admin.Id, new string[] { "Admin", "User" });
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
                    usrManager.Create(support, "adsfdg");
                    usrManager.AddToRoles(support.Id, new string[] { "Support", "User" });
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
                    usrManager.Create(user, "wUcheva$3a");
                    usrManager.AddToRoles(user.Id, new string[] { "User" });
                    allUsers.Add(user);
                }
            }

            for (int i = 1; i < 6; i++)
            {
                var userRequest = usrManager.FindByName("User" + i);
                var adminRequest = usrManager.FindByName("AdminUser" + 1);
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
                var userRequest = usrManager.FindByName("User" + i);
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


            var userForLog = usrManager.FindByName("User" + 1);

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
            var operations = new List<OperatingSystem>();


            operations.Add(new OperatingSystem
            {
                Name = "Windows ",
                Description = "Описание ",
                ImageFileId = image.Id,
                ServerTemplateName = "ServerTemplateName",
                Family = OperatingSystemFamily.Windows2012,
                DefaultAdminPassword = "Password123",
                DefaultAdminName = "Administrator",
                MinCoreCount = 2,
                MinHardDriveSize = 50,
                MinRamCount = 1024
            });
            operations.Add(new OperatingSystem
            {
                Name = "Ubuntu ",
                Description = "Описание ",
                ImageFileId = image.Id,
                ServerTemplateName = "ServerTemplateName",
                Family = OperatingSystemFamily.Ubuntu,
                DefaultAdminPassword = "Password123",
                DefaultAdminName = "Administrator",
                MinCoreCount = 1,
                MinHardDriveSize = 25,
                MinRamCount = 512
            });
            context.OperatingSystems.AddRange(operations);


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
                        OperatingSystemId = operations[random.Next(operations.Count)].Id,
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
                var valuesPayment = context.PaymentSystems;

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
                            Status = (PaymentStatus)random.Next(3),
                            PaymentSystemId = typePayment.Id
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

            for (int i = 0; i < 10; i++)
            {
                var iterUser = (i + 1 < 6) ? i + 1 : 5;
                var createVmOptions = new CreateVmOptions
                {
                    Cpu = 2,
                    HddGB = 300,
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
                    HardDriveSize = createVmOptions.HddGB,
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
            context.Commit();
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

            context.Commit();
            var gameServer = new GameServer
            {
                PaymentType = ServerPaymentType.Slot,
                VmId = allMachine[0].Id,
                SlotCount = 5,
                GameServerConfigurationId = gameServerConfig.Id,
                UserId = allUsers[0].Id,
                CreateDate = DateTime.UtcNow,
                DateExpire = DateTime.UtcNow.AddMonths(1)
            };
            context.GameServers.Add(gameServer);

            var phServer1 = new PhysicalServer { ProcessorName = "Intel® Xeon™ 1 Core", Price = (decimal)45.5 };
            var phServer2 = new PhysicalServer { ProcessorName = "Intel® Xeon™ 2 Core", Price = (decimal)85.5 };
            var opt1 = new PhysicalServerOption { Name = "5 GB SSD", Type = PhysicalServerOptionType.Hdd, Price = 50.5m };
            var opt2 = new PhysicalServerOption { Name = "25 GB SSD", Type = PhysicalServerOptionType.Hdd, Price = 50.5m };
            var opt3 = new PhysicalServerOption { Name = "1 GB DDR4", Type = PhysicalServerOptionType.Ram, Price = 50.5m };
            var opt4 = new PhysicalServerOption { Name = "2 GB DDR4", Type = PhysicalServerOptionType.Ram, Price = 50.5m };
            var opt5 = new PhysicalServerOption { Name = "4 GB DDR4", Type = PhysicalServerOptionType.Ram, Price = 50.5m };
            var opt6 = new PhysicalServerOption { Name = "8 GB DDR4", Type = PhysicalServerOptionType.Ram, Price = 50.5m };

            context.PhysicalServerOptions.Add(opt1);
            context.PhysicalServerOptions.Add(opt2);
            context.PhysicalServerOptions.Add(opt3);
            context.PhysicalServerOptions.Add(opt4);
            context.PhysicalServerOptions.Add(opt5);
            context.PhysicalServerOptions.Add(opt6);
            context.Commit();

            context.PhysicalServers.Add(phServer1);
            context.PhysicalServers.Add(phServer2);
            context.Commit();

            var avServOpt1 = new PhysicalServerOptionsAvailable { PhysicalServerId = phServer1.Id, OptionId = opt1.Id, IsDefault = true };
            var avServOpt2 = new PhysicalServerOptionsAvailable { PhysicalServerId = phServer1.Id, OptionId = opt2.Id };
            var avServOpt3 = new PhysicalServerOptionsAvailable { PhysicalServerId = phServer1.Id, OptionId = opt3.Id, IsDefault = true };
            var avServOpt4 = new PhysicalServerOptionsAvailable { PhysicalServerId = phServer1.Id, OptionId = opt4.Id };
            context.AvailableOptionsPhysicalServers.Add(avServOpt1);
            context.AvailableOptionsPhysicalServers.Add(avServOpt2);
            context.AvailableOptionsPhysicalServers.Add(avServOpt3);
            context.AvailableOptionsPhysicalServers.Add(avServOpt4);

            context.Commit();

            var fixedSubscriptions =
                context.SubscriptionVms.Where(s => s.SubscriptionType == SubscriptionType.Fixed).ToList();
            var allTarifs = context.Tariffs.ToList();
            foreach (var fixedSub in fixedSubscriptions)
            {
                var adminUser = allUsers.First(u => u.UserName == "AdminUser" + 1);
                var transaction = new BillingTransaction
                {
                    Id = Guid.NewGuid(),
                    CashAmount = -2000,
                    TransactionType = BillingTransactionType.TestPeriod,
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
            context.Commit();
            var usageSubscriptions = context.SubscriptionVms.Where(s => s.SubscriptionType == SubscriptionType.Usage).ToList();
            foreach (var usageSub in usageSubscriptions)
            {
                var adminUser = allUsers.First(u => u.UserName == "AdminUser" + 1);
                var transaction = new BillingTransaction
                {
                    Id = Guid.NewGuid(),
                    CashAmount = -2000,
                    TransactionType = BillingTransactionType.TestPeriod,
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

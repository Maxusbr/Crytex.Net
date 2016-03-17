using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Crytex.Core.Service;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Crytex.Data;
using Crytex.Model.Models;
using Crytex.Notification;
using Microsoft.Owin.Security.DataProtection;

namespace Crytex.Web
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Подключите здесь службу электронной почты для отправки сообщения электронной почты.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        private readonly ISmsSender _smsSender;

        public SmsService(ISmsSender smsSender)
        {
            _smsSender = smsSender;
        }

        public Task SendAsync(IdentityMessage message)
        {
            _smsSender.Send(message.Destination, message.Body);

            return Task.FromResult(0);
        }
    }

    // Настройка диспетчера пользователей приложения. UserManager определяется в ASP.NET Identity и используется приложением.
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly IServerConfig _config;

        public ApplicationUserManager(IUserStore<ApplicationUser> store, IServerConfig config)
            : base(store)
        {
            _config = config;


            this.UserValidator = new UserValidator<ApplicationUser>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Настройка логики проверки паролей
            this.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = false,
                RequireLowercase = false,
                RequireUppercase = false,
            };

            // Настройка параметров блокировки по умолчанию
            this.UserLockoutEnabledByDefault = true;
            this.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            this.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Регистрация поставщиков двухфакторной проверки подлинности. Для получения кода проверки пользователя в данном приложении используется телефон и сообщения электронной почты
            // Здесь можно указать собственный поставщик и подключить его.
            this.RegisterTwoFactorProvider("Код, полученный по телефону", new PhoneNumberTokenProvider<ApplicationUser>
            {
                MessageFormat = "Ваш код безопасности: {0}"
            });
            this.RegisterTwoFactorProvider("Код из сообщения", new EmailTokenProvider<ApplicationUser>
            {
                Subject = "Код безопасности",
                BodyFormat = "Ваш код безопасности: {0}"
            });
            this.EmailService = new EmailService();

            var smscruLogin = _config.GetSmscruLogin();
            var smscruPassword = _config.GetSmscruPassword();
            this.SmsService = new SmsService(new SmscruSender(smscruLogin, smscruPassword));

            // Uncomment if use fake sms sender
            //this.SmsService = new SmsService(new FakeSmsSender());

            var provider = new DpapiDataProtectionProvider("TestWebAPI");
            this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(protector: provider.Create("ASP.NET Identity"))
            {
                TokenLifespan = TimeSpan.FromDays(1),
            };


        }
    }

    // Настройка диспетчера входа для приложения.
    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}

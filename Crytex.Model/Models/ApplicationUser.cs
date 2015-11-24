﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace Crytex.Model.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

        public virtual UserInfo UserInfo { get; set; }

        [InverseProperty("User")]
        public virtual ICollection<HelpDeskRequest> HelpDeskRequests { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<HelpDeskRequestComment> HelpDeskRequestComments { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ServerTemplate> ServerTemplates { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BillingTransaction> BillingTransactions { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<CreditPaymentOrder> CreditPaymentOrders { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserVm> UserVms { get; set; }

        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string City { get; set; }
        public string Areas { get; set; }
        public string Address { get; set; }
        public string CodePhrase { get; set; }
        public TypeUser UserType { get; set; }
    }

    public  enum TypeUser
    {
        JuridicalPerson,
        PhysicalPerson
    }
}

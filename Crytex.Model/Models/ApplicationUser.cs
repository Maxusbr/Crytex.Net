﻿using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Crytex.Model.Models.Biling;
using System.ComponentModel.DataAnnotations;

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

        [InverseProperty("User")]
        public virtual ICollection<HelpDeskRequest> HelpDeskRequests { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<HelpDeskRequestComment> HelpDeskRequestComments { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<ServerTemplate> ServerTemplates { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<BillingTransaction> BillingTransactions { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<Payment> CreditPaymentOrders { get; set; }
        [InverseProperty("User")]
        public virtual ICollection<UserVm> UserVms { get; set; }

        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Patronymic { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string CodePhrase { get; set; }
        public TypeUser UserType { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool Deleted { get; set; }
        public bool IsBlocked { get; set; }
        public string Payer { get; set; }
        public string ContactPerson { get; set; }
        public decimal Balance { get; set; }
        public string Region { get; set; }
        public string CompanyName { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string JuridicalAddress { get; set; }
        public string MailAddress { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        
    }

    public  enum TypeUser
    {
        JuridicalPerson,
        PhysicalPerson
    }
}

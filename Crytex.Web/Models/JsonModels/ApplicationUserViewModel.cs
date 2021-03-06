﻿using System.ComponentModel.DataAnnotations;
using Crytex.Model.Models;

namespace Crytex.Web.Models.JsonModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public bool ChangePassword { get; set; }
        public  string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string CodePhrase { get; set; }
        public TypeUser UserType { get; set; }
        public string Payer { get; set; }
        public string ContactPerson { get; set; }
        public decimal Balance { get; set; }
        public bool IsBlocked { get; set; }
        public bool ValidateForCreationScenario()
        {
            if (string.IsNullOrEmpty(this.UserName)) return false;
            if (string.IsNullOrEmpty(this.Email)) return false;
            if (string.IsNullOrEmpty(this.Password)) return false;

            return true;
        }
        public string CompanyName { get; set; }
        public string INN { get; set; }
        public string KPP { get; set; }
        public string JuridicalAddress { get; set; }
        public string MailAddress { get; set; }
        public bool ValidateForJuridicalInfo()
        {
            if (UserType == TypeUser.PhysicalPerson) return true;
            return !string.IsNullOrEmpty(CompanyName) && !string.IsNullOrEmpty(INN) &&
                   !string.IsNullOrEmpty(KPP) && !string.IsNullOrEmpty(JuridicalAddress) &&
                   !string.IsNullOrEmpty(MailAddress);
        }

        public bool ValidateForEditingScenario()
        {
            if (string.IsNullOrEmpty(this.UserName) && string.IsNullOrEmpty(this.Email) && string.IsNullOrEmpty(this.Password))
            {
                return false;
            }

            return true;
        }
    }
}
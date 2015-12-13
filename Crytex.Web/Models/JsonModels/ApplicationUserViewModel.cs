using System.ComponentModel.DataAnnotations;
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

        public string Name { get; set; }
        public string Patronymic { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public string CodePhrase { get; set; }
        public TypeUser UserType { get; set; }
        public string Payer { get; set; }
        public string ContactPerson { get; set; }

        public bool ValidateForCreationScenario()
        {
            if (string.IsNullOrEmpty(this.UserName)) return false;
            if (string.IsNullOrEmpty(this.Email)) return false;
            if (string.IsNullOrEmpty(this.Password)) return false;

            return true;
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
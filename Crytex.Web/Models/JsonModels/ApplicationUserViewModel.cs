using System.ComponentModel.DataAnnotations;
namespace Crytex.Web.Models.JsonModels
{
    public class ApplicationUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }

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
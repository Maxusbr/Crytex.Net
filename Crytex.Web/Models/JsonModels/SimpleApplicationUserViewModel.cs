using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models
{
    public class SimpleApplicationUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
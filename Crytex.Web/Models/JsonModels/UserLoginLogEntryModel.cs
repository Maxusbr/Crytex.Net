using System;

namespace Crytex.Web.Models.JsonModels
{
    class UserLoginLogEntryModel
    {
        public string UserId { get; set; }
        public DateTime LoginDate { get; set; }
        public string IpAddress { get; set; }
        public bool WithDataSaving { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Crytex.Web.Models.JsonModels
{
    public class NewsViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }
        public DateTime CreateTime { get; set; }
        public String Body { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}
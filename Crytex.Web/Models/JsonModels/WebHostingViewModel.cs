using System;
using TypeLite;

namespace Crytex.Web.Models.JsonModels
{

    [TsClass]
    public class WebHostingViewModel
    {
        public Guid Id { get; set; }
        public string TariffName { get; set; }
    }
}
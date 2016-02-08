using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using Microsoft.AspNet.SignalR.Hubs;

namespace Crytex.Web.Areas.User.Controllers
{
    [AllowAnonymous]
    public class TariffController : UserCrytexController
    {
        private readonly ITariffInfoService _tariffInfoService;

        public TariffController(ITariffInfoService tariffInfoService)
        {
            this._tariffInfoService = tariffInfoService;
        }

        /// <summary>
        /// Получение тарифа по типу виртуализации
        /// </summary>
        /// <param name="virtualization"></param>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        // GET: api/Admin/AdminTariff/0
        [ResponseType(typeof(TariffViewModel))]

        public IHttpActionResult Get(TypeVirtualization? virtualization = null,
            OperatingSystemFamily? operatingSystem = null)
        {
            if (virtualization != null)
            {
                var tariff = _tariffInfoService.GetTariffByType(virtualization.Value, operatingSystem.Value);
                var viewTariff = AutoMapper.Mapper.Map<TariffViewModel>(tariff);
                return Ok(viewTariff);
            }
            else
            {
                var tariffs = _tariffInfoService.GetTariffs();
                var viewTariffs = AutoMapper.Mapper.Map<List<Tariff>, List<TariffViewModel>>(tariffs);
                return Ok(viewTariffs);
            }
        }

    }
}
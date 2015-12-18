using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin
{
    public class AdminTariffController : AdminCrytexController
    {
        private readonly ITariffInfoService _tariffInfoService;

        public AdminTariffController(ITariffInfoService tariffInfoService)
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
        public IHttpActionResult Get(TypeVirtualization? virtualization = null, TypeOfOperatingSystem? operatingSystem = null)
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
                var viewTariffs = AutoMapper.Mapper.Map<List<Tariff>,List<TariffViewModel>>(tariffs);
                return Ok(viewTariffs);
            }
        }

        /// <summary>
        /// Создание нового тарифа
        /// </summary>
        /// <param name="tariff"></param>
        /// <returns></returns>
        // POST: api/Admin/AdminTariff
        public IHttpActionResult Post(TariffViewModel tariff)
        {
            if (!ModelState.IsValid || tariff == null)
                return BadRequest(ModelState);

            var modelTariff = AutoMapper.Mapper.Map<Tariff>(tariff);

            var newTariff = _tariffInfoService.CreateTariff(modelTariff);

            return Ok(newTariff);
        }

        /// <summary>
        /// Обновление тарифа
        /// </summary>
        /// <param name="tariff"></param>
        /// <returns></returns>
        // PUT: api/Admin/AdminTariff
        public IHttpActionResult Put(TariffViewModel tariff)
        {
            if (!ModelState.IsValid || tariff == null)
                return BadRequest(ModelState);

            var modelTariff = AutoMapper.Mapper.Map<Tariff>(tariff);

            _tariffInfoService.UpdateTariff(modelTariff);

            return Ok();
        }

        /// <summary>
        /// Получение итоговой цены по параметрам и указанному тарифу
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="HDD"></param>
        /// <param name="SSD"></param>
        /// <param name="RAM512"></param>
        /// <param name="tariffId"></param>
        /// <returns></returns>
        // GET: api/Admin/AdminTariff/Total
        [Route("api/Admin/AdminTariff/Total")]
        [ResponseType(typeof(decimal))]
        public IHttpActionResult GetTotalPrice(decimal processor, decimal HDD, decimal SSD, decimal RAM512, decimal load10Percent, Guid tariffId)
        {
            var tariff = _tariffInfoService.GetTariffById(tariffId);
            decimal totalPrice = _tariffInfoService.CalculateTotalPrice(processor, HDD, SSD, RAM512, load10Percent, tariff);            
            return Ok(totalPrice);
        }

    }
}

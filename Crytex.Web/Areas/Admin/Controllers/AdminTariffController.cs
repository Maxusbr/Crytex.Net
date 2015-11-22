using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Service.Model;
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
        /// <returns></returns>
        // GET: api/Admin/AdminTariff/0
        [ResponseType(typeof(TariffViewModel))]
        public IHttpActionResult Get(TypeVirtualization virtualization)
        {
            var tariff = _tariffInfoService.GetTariffByVirtualization(virtualization);
            var viewTariff = AutoMapper.Mapper.Map<TariffViewModel>(tariff);           
            return Ok(viewTariff);
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
        [ResponseType(typeof(double))]
        public IHttpActionResult GetTotalPrice(double processor, double HDD, double SSD, double RAM512, Guid tariffId)
        {
            var tariff = _tariffInfoService.GetTariffById(tariffId);
            double totalPrice = processor * tariff.Processor1 +
                                HDD * tariff.HDD1 +
                                SSD * tariff.SSD1 +
                                RAM512 * tariff.RAM512;
            return Ok(totalPrice);
        }

    }
}

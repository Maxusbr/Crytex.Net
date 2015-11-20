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
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;

namespace Crytex.Web.Areas.Admin
{
    public class AdminStatisticController : AdminCrytexController
    {
        private readonly IStatisticService _statisticService;

        public AdminStatisticController(IStatisticService statisticService)
        {
            this._statisticService = statisticService;
        }

        
        /// <summary>
        /// Получение списка Statistic
        /// </summary>
        /// <param name="statisticType"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        // GET: api/AdminStatistic
        [ResponseType(typeof(PageModel<StatisticViewModel>))]
        public IHttpActionResult Get(int pageNumber, int pageSize, StatisticType? statisticType = null, DateTime? dateFrom = null, DateTime? dateTo = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var statistics = _statisticService.GetAllPageStatistics(pageNumber, pageSize, statisticType, dateFrom, dateTo);
            var viewStatistics = AutoMapper.Mapper.Map<PageModel<StatisticViewModel>>(statistics);

            return Ok(viewStatistics);
        }

        /// <summary>
        /// Получение StatisticSummary
        /// </summary>
        // GET: api/Admin/AdminStatistic/method/summary
        [ResponseType(typeof(StatisticSummary))]
        [Route("api/Admin/AdminStatistic/method/summary")]
        public IHttpActionResult GetSummary()
        {
            var statisticSummary = _statisticService.GetSummary();
            return Ok(statisticSummary);
        }

        /// <summary>
        /// Получение Statistic по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/AdminStatistic/5
        [ResponseType(typeof(StatisticViewModel))]
        public IHttpActionResult Get(int id)
        {
            var statistic = _statisticService.GetStatisticById(id);
            var viewStatistic = AutoMapper.Mapper.Map<StatisticViewModel>(statistic);

            return Ok(viewStatistic);
        }

        /// <summary>
        /// Создание нового Statistic
        /// </summary>
        /// <param name="statistic"></param>
        /// <returns></returns>
        // POST: api/AdminStatistic
        public IHttpActionResult Post([FromBody]StatisticViewModel statistic)
        {
            if (!ModelState.IsValid || statistic == null)
                return BadRequest(ModelState);

            var modelStatistic = AutoMapper.Mapper.Map<Statistic>(statistic);
            var newStatistic = _statisticService.CreateStatistic(modelStatistic);

            return Ok(newStatistic);
        }

        /// <summary>
        /// Обновление Statistic
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        // PUT: api/AdminStatistic/5
        public IHttpActionResult Put([FromBody]StatisticViewModel updateStatistic)
        {
            if (!ModelState.IsValid || updateStatistic == null)
                return BadRequest(ModelState);

            var modelStatistic = AutoMapper.Mapper.Map<Statistic>(updateStatistic);
            _statisticService.UpdateStatistic(modelStatistic);

            return Ok(modelStatistic);
        }

        /// <summary>
        /// Удаление Statistic по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/AdminStatistic/5
        public IHttpActionResult Delete(int id)
        {
            var statistic = _statisticService.GetStatisticById(id);
            if (statistic == null)
                return BadRequest();

            _statisticService.DeleteStatisticById(statistic.Id);

            return Ok();
        }
    }
}

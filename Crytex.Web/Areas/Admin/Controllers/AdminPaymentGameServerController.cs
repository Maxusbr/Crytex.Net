using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Mvc;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminPaymentGameServerController : AdminCrytexController
    {
        private readonly IGameServerService _gameServerService;

        public AdminPaymentGameServerController(IGameServerService gameServerService)
        {
            _gameServerService = gameServerService;
        }

        /// <summary>
        /// Получение списка PaymentGameServer
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<PaymentGameServerViewModel>))]
        // GET: api/CreditPaymentOrder
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri]SearchPaymentGameServerParams filter)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            var page = this._gameServerService.GetPage(pageNumber, pageSize, filter);
            var viewModel = AutoMapper.Mapper.Map<PageModel<PaymentGameServerViewModel>>(page);

            return Ok(viewModel);
        }
    }
}
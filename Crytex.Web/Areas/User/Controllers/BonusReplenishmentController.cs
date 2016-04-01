using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using AutoMapper;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.User.Controllers
{
    public class BonusReplenishmentController : UserCrytexController
    {
        private readonly IDiscountService _discountService;

        public BonusReplenishmentController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<BonusReplenishmentViewModel>))]
        public IHttpActionResult Get()
        {
            var allBonusReplenishments = _discountService.GetAllBonusReplenishments();
            var models = Mapper.Map<IEnumerable<BonusReplenishmentViewModel>>(allBonusReplenishments);

            return Ok(allBonusReplenishments);
        }
    }
}
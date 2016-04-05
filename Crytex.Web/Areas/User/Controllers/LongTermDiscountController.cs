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
    public class LongTermDiscountController : UserCrytexController
    {
        private readonly IDiscountService _discountService;

        public LongTermDiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        [ResponseType(typeof(IEnumerable<LongTermDiscountViewModel>))]
        public IHttpActionResult Get()
        {
            var discounts = _discountService.GetAllLongTermDiscounts();
            var models = Mapper.Map<IEnumerable<LongTermDiscountViewModel>>(discounts);

            return Ok(models);
        }
    }
}
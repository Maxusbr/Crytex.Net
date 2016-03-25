using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminLongTermDiscountController : AdminCrytexController
    {
        private readonly IDiscountService _discountService;

        public AdminLongTermDiscountController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var discounts = _discountService.GetAllLongTermDiscounts();
            var models = Mapper.Map<IEnumerable<LongTermDiscountViewModel>>(discounts);

            return Ok(models);
        }

        [HttpPost]
        public IHttpActionResult Post(LongTermDiscountViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var newDiscount = Mapper.Map<LongTermDiscount>(model);
            newDiscount = _discountService.CreateNewLongTermDiscount(newDiscount);

            return Ok(new {Id = newDiscount.Id});
        }

        [HttpPut]
        public IHttpActionResult Put(LongTermDiscountViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var updatedDiscount = Mapper.Map<LongTermDiscount>(model);
            _discountService.UpdateLongTermDiscount(updatedDiscount);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _discountService.DeleteLongTermDiscount(id);

            return Ok();
        }
    }
}
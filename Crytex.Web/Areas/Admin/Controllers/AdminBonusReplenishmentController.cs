using System.Collections;
using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminBonusReplenishmentController : AdminCrytexController
    {
        private readonly IDiscountService _discountService;

        public AdminBonusReplenishmentController(IDiscountService discountService)
        {
            _discountService = discountService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var allBonusReplenishments = _discountService.GetAllBonusReplenishments();
            var models = Mapper.Map<IEnumerable<BonusReplenishmentViewModel>>(allBonusReplenishments);

            return Ok(allBonusReplenishments);
        }

        [HttpPost]
        public IHttpActionResult Post(BonusReplenishmentViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var newReplenishment = Mapper.Map<BonusReplenishment>(model);
            newReplenishment = _discountService.CreateNewBonusReplenishment(newReplenishment);

            return Ok(new {Id = newReplenishment.Id});

        }

        [HttpPut]
        public IHttpActionResult Put(BonusReplenishmentViewModel model)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var updatedReplenishment = Mapper.Map<BonusReplenishment>(model);
            _discountService.UpdateBonusReplenishment(updatedReplenishment);

            return Ok();
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            _discountService.DeleteBonusReplenishment(id);

            return Ok();
        }
    }
}
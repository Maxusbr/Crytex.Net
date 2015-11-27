using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin
{
    public class AdminDiscountController : AdminCrytexController
    {
        private readonly IDiscountService _discountService;

        public AdminDiscountController(IDiscountService discountService)
        {
            this._discountService = discountService;
        }

        /// <summary>
        /// Получение всех Discount
        /// </summary>
        /// <returns></returns>
        // GET: api/AdminDiscount
        [ResponseType(typeof(IEnumerable<DiscountViewModel>))]
        public IHttpActionResult Get()
        {
            var discounts = this._discountService.GetAllDiscounts();
            var viewModel = AutoMapper.Mapper.Map<IEnumerable<DiscountViewModel>>(discounts);

            return Ok(viewModel);
        }

        /// <summary>
        /// Создание нового Discount
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/AdminDiscount
        public IHttpActionResult Post([FromBody]DiscountViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var discount = AutoMapper.Mapper.Map<Discount>(model);
            var newdiscount = this._discountService.CreateDiscount(discount);

            return Ok(newdiscount);
        }

        /// <summary>
        /// Обновление Discount
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        // PUT: api/AdminDiscount/5
        public IHttpActionResult Put(int id, [FromBody]DiscountViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var discount = AutoMapper.Mapper.Map<Discount>(model);
            discount.Id = id;
            this._discountService.UpdateDiscount(discount);

            return Ok();
        }

        /// <summary>
        /// Удаление Discount по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/AdminDiscount/5
        public IHttpActionResult Delete(int id)
        {
            this._discountService.DeleteDiscountById(id);
            return Ok();
        }

        /// <summary>
        /// Обновление DiscountDisable
        /// </summary>
        /// <param name="disable"></param>
        /// <param name="typeDiscount"></param>
        /// <returns></returns>
        // POST: api/AdminDiscount/BlockType/5
        [HttpPost]//api/Admin/AdminStatistic/method/summary
        [Route("api/Admin/AdminDiscount/BlockType/{typeDiscount}/{disable}")]
        public IHttpActionResult BlockType(TypeDiscount typeDiscount, bool disable)
        {
            this._discountService.UpdateDisable(disable, typeDiscount);

            return Ok(disable);
        }
    }
}

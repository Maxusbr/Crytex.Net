using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Models.Biling;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;
using PagedList;

namespace Crytex.Web.Areas.User.Controllers
{
    public class BillingTransactionController : UserCrytexController
    {
        private readonly IBilingService _billingService;

        public BillingTransactionController(IBilingService billingService)
        {
            this._billingService = billingService;
        }

        /// <summary>
        /// Получение списка транзакций пользователя
        /// </summary>
        /// <param name="searchParams"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        // GET: api/Admin/AdminBillingTransaction
        [ResponseType(typeof(PageModel<BillingViewModel>))]
        public IHttpActionResult Get(int pageNumber, int pageSize, [FromUri]BillingSearchParamsViewModel searchParams = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IPagedList<BillingTransaction> transactions =
                new PagedList<BillingTransaction>(new List<BillingTransaction>(), pageNumber, pageSize);

            searchParams.UserId = this.CrytexContext.UserInfoProvider.GetUserId();

            if (searchParams != null)
            {
                var billingParams = AutoMapper.Mapper.Map<BillingSearchParams>(searchParams);
                transactions = _billingService.GetPageBillingTransaction(pageNumber, pageSize, billingParams);
            }
            else
            {
                transactions = _billingService.GetPageBillingTransaction(pageNumber, pageSize);
            }

            var viewTransactions = AutoMapper.Mapper.Map<PageModel<BillingViewModel>>(transactions);
            return Ok(viewTransactions);
        }


        /// <summary>
        /// Получение BillingTransaction по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Admin/AdminBillingTransaction/1
        [ResponseType(typeof(BillingViewModel))]
        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
                return BadRequest("Invalid Guid format");
            var transaction = _billingService.GetTransactionById(guid);
            var viewTransaction = AutoMapper.Mapper.Map<BillingViewModel>(transaction);
            return Ok(viewTransaction);
        }
    }
}

using System;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Service.Model;
using Crytex.Model.Exceptions;

namespace Crytex.Web.Areas.User
{
    public class HelpDeskRequestController : UserCrytexController
    {
        private ISecureHelpDeskRequestService _helpDeskRequestService { get; }

        public HelpDeskRequestController(ISecureHelpDeskRequestService helpDeskRequestService)
        {
            this._helpDeskRequestService = helpDeskRequestService;
        }

        /// <summary>
        /// Получение списка HelpDeskRequest
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<HelpDeskRequestViewModel>))]
        public IHttpActionResult Get(int pageNumber, int pageSize, HelpDeskRequestFilter filter = HelpDeskRequestFilter.All)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");
            if (!Enum.IsDefined(typeof(HelpDeskRequestFilter), filter))
                return BadRequest("Filter wrong type");

            var page = this._helpDeskRequestService.GetPage(pageNumber, pageSize, filter);
            var viewModel = AutoMapper.Mapper.Map<PageModel<HelpDeskRequestViewModel>>(page);

            return Ok(viewModel);
        }

        /// <summary>
        /// Получение HelpDeskRequest по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/HelpDeskRequest/5
        [ResponseType(typeof(HelpDeskRequestViewModel))]
        public IHttpActionResult Get(int id)
        {
            var request = this._helpDeskRequestService.GeById(id);
            var model = AutoMapper.Mapper.Map<HelpDeskRequestViewModel>(request);

            return Ok(model);
        }

        /// <summary>
        /// Создание нового HelpDeskRequest для пользователя
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        // POST: api/HelpDeskRequest
        public IHttpActionResult Post([FromBody]HelpDeskRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = CrytexContext.UserInfoProvider.GetUserId();
            var requestToCreate = AutoMapper.Mapper.Map<HelpDeskRequest>(model);
            requestToCreate.UserId = userId;
            var newRequest = _helpDeskRequestService.CreateNew(requestToCreate);

            return Ok(new { id = newRequest.Id });
        }

        /// <summary>
        /// Обновление HelpDeskRequest по Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult Put(int id, [FromBody]HelpDeskRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userRequest = AutoMapper.Mapper.Map<HelpDeskRequest>(model);
            userRequest.Id = id;
            this._helpDeskRequestService.Update(userRequest);

            return Ok();
        }

        /// <summary>
        /// Удаление HelpDeskRequest по Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        // DELETE: api/HelpDeskRequest/5
        public IHttpActionResult Delete(int id)
        {
            this._helpDeskRequestService.DeleteById(id);
            return Ok();
        }
    }
}

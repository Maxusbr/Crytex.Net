using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System.Web.Http;
using Crytex.Web.Areas.Admin.Controllers;

namespace Crytex.Web.Controllers.Api
{
    public class AdminHelpDeskRequestController : AdminCrytexController
    {
        private IHelpDeskRequestService _helpDeskRequestService { get; }

        public AdminHelpDeskRequestController(IHelpDeskRequestService helpDeskRequestService)
        {
            this._helpDeskRequestService = helpDeskRequestService;
        }

        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");

            var page = this._helpDeskRequestService.GetPage(pageNumber, pageSize);
            var viewModel = AutoMapper.Mapper.Map<PageModel<HelpDeskRequestViewModel>>(page);

            return Ok(viewModel);
        }

        // GET: api/HelpDeskRequest/5
        public IHttpActionResult Get(int id)
        {
            var request = this._helpDeskRequestService.GeById(id);
            var model = AutoMapper.Mapper.Map<HelpDeskRequestViewModel>(request);

            return Ok(model);
        }


        // POST: api/HelpDeskRequest
        public IHttpActionResult Post([FromBody]HelpDeskRequestViewModel model, string userId = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (string.IsNullOrEmpty(userId))
            {
                userId = CrytexContext.UserInfoProvider.GetUserId();
            }
            
            var newRequest = _helpDeskRequestService.CreateNew(model.Summary, model.Details, userId);

            return Ok(new { id = newRequest.Id });
        }

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



        [HttpDelete]
        // DELETE: api/HelpDeskRequest/5
        public IHttpActionResult Delete(int id)
        {
            this._helpDeskRequestService.DeleteById(id);
            return Ok();
        }
    }
}

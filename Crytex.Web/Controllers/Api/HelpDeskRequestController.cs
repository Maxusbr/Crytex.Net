using DataAnnotationsExtensions;
using PagedList;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Crytex.Web.Controllers.Api
{
    public class HelpDeskRequestController : CrytexApiController
    {
        private readonly IHelpDeskRequestService _helpDeskRequestService;

        public HelpDeskRequestController(IHelpDeskRequestService helpDeskRequstService)
        {
            this._helpDeskRequestService = helpDeskRequstService;
        }

        [HttpGet]
        public HttpResponseMessage Get(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                this.ModelState.AddModelError("", "PageNumber and PageSize must be grater than 1");
            }
            else
            {
                var page = this._helpDeskRequestService.GetPage(pageNumber, pageSize);
                var viewModel = AutoMapper.Mapper.Map<PageModel<HelpDeskRequestViewModel>>(page);
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // GET: api/HelpDeskRequest/5
        [HttpGet]
        public HelpDeskRequestViewModel GetById(int id)
        {
            var request =  this._helpDeskRequestService.GeById(id);
            var model = AutoMapper.Mapper.Map<HelpDeskRequestViewModel>(request);

            return model;
        }

        [HttpPost]
        public HttpResponseMessage Update(int id, HelpDeskRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userRequest = AutoMapper.Mapper.Map<HelpDeskRequest>(model);
                userRequest.Id = id;
                this._helpDeskRequestService.Update(userRequest);

                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // POST: api/HelpDeskRequest
        [HttpPost]
        public HttpResponseMessage Create(HelpDeskRequestViewModel model)
        {
            if(ModelState.IsValid){
                var userId = this.CrytexContext.UserInfoProvider.GetUserId();
                var newRequest = this._helpDeskRequestService.CreateNew(model.Summary, model.Details, userId);

                return Request.CreateResponse(HttpStatusCode.Created, new { id = newRequest.Id});
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        [HttpDelete]
        // DELETE: api/HelpDeskRequest/5
        public HttpResponseMessage Delete(int id)
        {
            this._helpDeskRequestService.DeleteById(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

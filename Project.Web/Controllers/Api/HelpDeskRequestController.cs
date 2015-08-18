using DataAnnotationsExtensions;
using PagedList;
using Project.Model.Models;
using Project.Service.IService;
using Project.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Project.Web.Controllers.Api
{
    public class HelpDeskRequestController : ApiController
    {
        private readonly IHelpDeskRequestService _helpDeskRequestService;
        private readonly ApplicationUserManager _userManager;

        public HelpDeskRequestController(IHelpDeskRequestService helpDeskRequstService, ApplicationUserManager userManager)
        {
            this._helpDeskRequestService = helpDeskRequstService;
            this._userManager = userManager;
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
                var userName = this.User.Identity.Name;
                var user = this._userManager.Users.Single(u => u.UserName == userName);
                var newRequest = this._helpDeskRequestService.CreateNew(model.Summary, model.Details, user.Id);

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

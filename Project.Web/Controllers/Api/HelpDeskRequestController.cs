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
                var modelItems = ToModelItems(page);
                var viewModel = new HelpRequestPageViewModel
                {
                    Items = modelItems,
                    TotalPages = page.PageCount,
                    TotalRows = page.TotalItemCount
                };
                return Request.CreateResponse(HttpStatusCode.OK, viewModel);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        private IEnumerable<HelpDeskRequestViewModel> ToModelItems(IPagedList<HelpDeskRequest> page)
        {
            return page.Select(i => new HelpDeskRequestViewModel
            {
                Details = i.Details,
                Summary = i.Summary,
                CreationDate = i.CreationDate,
                Status = i.Status,
                Id = i.Id
            });
        }

        // GET: api/HelpDeskRequest/5
        [HttpGet]
        public HelpDeskRequestViewModel GetById(int id)
        {
            var request =  this._helpDeskRequestService.GeById(id);
            var model = new HelpDeskRequestViewModel
            {
                Status = request.Status,
                Summary = request.Summary,
                Details = request.Details,
                CreationDate = request.CreationDate
            };

            return model;
        }

        [HttpPost]
        public HttpResponseMessage Update(int id, HelpDeskRequestViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userRequest = new HelpDeskRequest
                {
                    Id = id,
                    Details = model.Details,
                    Status = model.Status,
                    Summary = model.Summary
                };
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
                var userId = "test"; // Temporary solution !
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

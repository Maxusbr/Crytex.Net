using Crytex.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;

namespace Crytex.Web.Controllers.Api
{
    public class HelpDeskRequestCommentController : CrytexApiController
    {
        private readonly IHelpDeskRequestService _helpDeskRequestService;

        public HelpDeskRequestCommentController(IHelpDeskRequestService helpDeskRequstService)
        {
            this._helpDeskRequestService = helpDeskRequstService;
        }

        // GET: api/HelpDeskRequestComment/5
        [HttpGet]
        public IEnumerable<HelpDeskRequestCommentViewModel> GetAllByRequestId(int id)
        {
            var comments = this._helpDeskRequestService.GetCommentsByRequestId(id).ToList();
            var model = AutoMapper.Mapper.Map<List<HelpDeskRequestComment>, List<HelpDeskRequestCommentViewModel>>(comments);

            return model;
        }

        // POST: api/HelpDeskRequestComment/id
        [HttpPost]
        public HttpResponseMessage Create(int id, HelpDeskRequestCommentViewModel model)
        {
            if(this.ModelState.IsValid)
            {
                var userId = this.CrytexContext.UserInfoProvider.GetUserId();
                var newComment = this._helpDeskRequestService.CreateComment(id, model.Comment, userId);
                return Request.CreateResponse(HttpStatusCode.Created, new { id = newComment.Id});
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // PUT: api/HelpDeskRequestComment/id
        [HttpPut]
        public HttpResponseMessage Update(int id, HelpDeskRequestCommentViewModel model)
        {
            if(this.ModelState.IsValid)
            {
                this._helpDeskRequestService.UpdateComment(id, model.Comment);
                return Request.CreateResponse(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        // DELETE: api/HelpDeskRequestComment/5
        [HttpDelete]
        public HttpResponseMessage Delete(int id)
        {
            this._helpDeskRequestService.DeleteCommentById(id);
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

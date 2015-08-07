using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Web.Models.JsonModels;
using Project.Model.Models;

namespace Project.Web.Controllers.Api
{
    public class HelpDeskRequestCommentController : ApiController
    {
        private readonly IHelpDeskRequestService _helpDeskRequestService;
        private readonly ApplicationUserManager _userManager;

        public HelpDeskRequestCommentController(IHelpDeskRequestService helpDeskRequstService, ApplicationUserManager userManager)
        {
            this._helpDeskRequestService = helpDeskRequstService;
            this._userManager = userManager;
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
                var userName = this.User.Identity.Name;
                var user = this._userManager.Users.Single(u => u.UserName == userName);
                var newComment = this._helpDeskRequestService.CreateComment(id, model.Comment, user.Id);
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
                var userName = this.User.Identity.Name;
                var user = this._userManager.Users.Single(u => u.UserName == userName);
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

﻿ using Crytex.Service.IService;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
 using System.Web.Http.Description;
 using Crytex.Web.Models.JsonModels;
using Crytex.Model.Models;


namespace Crytex.Web.Areas.Admin
{
    public class AdminHelpDeskRequestCommentController : AdminCrytexController
    {
        private readonly IHelpDeskRequestService _helpDeskRequestService;

        public AdminHelpDeskRequestCommentController(IHelpDeskRequestService helpDeskRequstService)
        {
            this._helpDeskRequestService = helpDeskRequstService;
        }

        /// <summary>
        /// Получение HelpDeskRequestComment по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/HelpDeskRequestComment/5
        [ResponseType(typeof(List<HelpDeskRequestCommentViewModel>))]
        public IHttpActionResult Get(int id)
        {
            var comments = this._helpDeskRequestService.GetCommentsByRequestId(id).ToList();
            var model = AutoMapper.Mapper.Map<List<HelpDeskRequestComment>, List<HelpDeskRequestCommentViewModel>>(comments);

            return Ok(model);
        }

        /// <summary>
        /// Создание нового HelpDeskRequestComment
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        // POST: api/HelpDeskRequestComment/id
        public IHttpActionResult Post(int id, [FromBody]HelpDeskRequestCommentViewModel model, string userId = null)
        {
            if(!this.ModelState.IsValid)
            {
                BadRequest(ModelState);
            }

            if(string.IsNullOrEmpty(userId))
            {
                userId = this.CrytexContext.UserInfoProvider.GetUserId();
            }

            var newComment = this._helpDeskRequestService.CreateComment(id, model.Comment, userId);

            return Ok(new { id = newComment.Id });
        }

        /// <summary>
        /// Обновление HelpDeskRequestComment по id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        // PUT: api/HelpDeskRequestComment/id
        public IHttpActionResult Put(int id, [FromBody]HelpDeskRequestCommentViewModel model)
        {
            if(!this.ModelState.IsValid)
            {
                BadRequest(ModelState);
            }
            this._helpDeskRequestService.UpdateComment(id, model.Comment);
            return Ok();
        }

        /// <summary>
        /// Удаление HelpDeskRequestComment по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/HelpDeskRequestComment/5
        public IHttpActionResult Delete(int id)
        {
            this._helpDeskRequestService.DeleteCommentById(id);
            return Ok();
        }
    }
}

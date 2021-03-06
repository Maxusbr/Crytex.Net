﻿using Crytex.Service.IService;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Web.Models.JsonModels;


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
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        // GET: api/HelpDeskRequestComment/5
        [ResponseType(typeof(PageModel<HelpDeskRequestCommentViewModel>))]
        public IHttpActionResult Get(int id, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");

            var comments = this._helpDeskRequestService.GetPageCommentsByRequestId(id, pageNumber, pageSize);
            var model = AutoMapper.Mapper.Map<PageModel<HelpDeskRequestCommentViewModel>>(comments);

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

            var modelViewComment = AutoMapper.Mapper.Map<HelpDeskRequestCommentViewModel>(newComment);

            return Ok(modelViewComment);
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

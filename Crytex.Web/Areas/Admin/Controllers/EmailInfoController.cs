using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Data.Migrations;
using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PagedList;

namespace Crytex.Web.Areas.Admin
{
    public class EmailInfoController : AdminCrytexController
    {
        private readonly IEmailInfoService _emailInfoService;

        public EmailInfoController(IEmailInfoService emailInfoService)
        {
            this._emailInfoService = emailInfoService;
        }


        /// <summary>
        /// Получение списка EmailInfo
        /// </summary>
        /// <param name="searchParams"></param>
        /// <returns></returns>
        // GET: api/EmailInfo
        [ResponseType(typeof(List<EmailInfoesViewModel>))]
        public IHttpActionResult Get([FromUri]SearchEmailParams searchParams = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var emails = this._emailInfoService.GetEmails(searchParams);
            var viewEmailInfoes = AutoMapper.Mapper.Map<List<EmailInfoesViewModel>>(emails);
            return Ok(viewEmailInfoes);
        }

        /// <summary>
        /// Получение EmailInfo по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/EmailInfo/5
        [ResponseType(typeof(EmailInfoesViewModel))]
        public IHttpActionResult Get(int id)
        {
            var emailInfo = _emailInfoService.GetEmail(id);
            var viewemailInfo = AutoMapper.Mapper.Map<EmailInfoesViewModel>(emailInfo);
            return Ok(viewemailInfo);
        }

        /// <summary>
        /// Удаление EmailInfo по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/EmailInfo/5
        public IHttpActionResult Delete(int id)
        {
            _emailInfoService.DeleteEmail(id);
            return Ok();
        }
    }
}

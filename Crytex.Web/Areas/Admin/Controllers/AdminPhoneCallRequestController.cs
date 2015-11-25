using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminPhoneCallRequestController : AdminCrytexController
    {
        private readonly IPhoneCallRequestService _phoneCallRequestService;

        public AdminPhoneCallRequestController(IPhoneCallRequestService phoneCallRequestService)
        {
            this._phoneCallRequestService = phoneCallRequestService;
        }

        public IHttpActionResult Get(int id)
        {
            var request = this._phoneCallRequestService.GetById(id);
            var model = AutoMapper.Mapper.Map<PhoneCallRequestViewModel>(request);

            return Ok(model);
        }

        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                return BadRequest("PageNumber and PageSize must be grater than 1");
            }

            var page = this._phoneCallRequestService.GetPage(pageNumber, pageSize);
            var pageModel = AutoMapper.Mapper.Map<PageModel<PhoneCallRequestViewModel>>(page);

            return Ok(pageModel);
        }

        public IHttpActionResult Put(int id, PhoneCallRequestEditViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var request = AutoMapper.Mapper.Map<PhoneCallRequest>(model);
            this._phoneCallRequestService.Update(id, request);

            return Ok();
        }
    }
}

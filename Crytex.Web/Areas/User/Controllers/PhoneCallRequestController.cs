using Crytex.Model.Models;
using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Crytex.Web.Areas.User.Controllers
{
    public class PhoneCallRequestController : UserCrytexController
    {
        private readonly IPhoneCallRequestService _phoneCallRequestService;

        public PhoneCallRequestController(IPhoneCallRequestService phoneCallRequestService)
        {
            this._phoneCallRequestService = phoneCallRequestService;
        }

        public IHttpActionResult Post(PhoneCallRequestViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var requestToCreate = AutoMapper.Mapper.Map<PhoneCallRequest>(model);
            var createdRequest = this._phoneCallRequestService.Create(requestToCreate);

            return Ok(new { id = createdRequest.Id });
        }
    }
}

using System.Collections.Generic;
using System.Web.Http;
using AutoMapper;
using Crytex.Model.Models;
using Crytex.Web.Models.JsonModels;
using Sample.Service.IService;

namespace Crytex.Web.Controllers.Api
{
    public class MessageController : CrytexApiController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // GET: api/Message
        public IHttpActionResult Get()
        {
            var viewModel = Mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(_messageService.GetAll());
            return Ok(viewModel);
        }

        // GET: api/Message/5
        public IHttpActionResult Get(int id)
        {
            return Ok("value");
        }

        // POST: api/Message
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Message/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Message/5
        public void Delete(int id)
        {
        }
    }
}

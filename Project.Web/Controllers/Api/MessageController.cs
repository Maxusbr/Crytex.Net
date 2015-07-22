using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Project.Model.Models;
using Project.Web.Models.JsonModels;
using Sample.Service.IService;

namespace Project.Web.Controllers.Api
{
    public class MessageController : ApiController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        // GET: api/Message
        public IEnumerable<MessageViewModel> Get()
        {
            return Mapper.Map<IEnumerable<Message>, IEnumerable<MessageViewModel>>(_messageService.GetAll()); 
        }

        // GET: api/Message/5
        public string Get(int id)
        {
            return "value";
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

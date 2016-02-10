using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Crytex.Model.Enums;
using Crytex.Service.IService;
using Crytex.Service.Model;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminPhysicalServerOptionsController : AdminCrytexController
    {
        private readonly IPhysicalServerService _serverService;

        public AdminPhysicalServerOptionsController(IPhysicalServerService serverService)
        {
            _serverService = serverService;
        }

        /// <summary>
        /// Список опций
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [ResponseType(typeof(PageModel<PhysicalServerOptionViewModel>))]
        [HttpGet]
        public IHttpActionResult Get(int pageNumber, int pageSize)
        {
            var options = _serverService.GetPagePhysicalServerOption(pageNumber, pageSize);
            var viewModel = AutoMapper.Mapper.Map<PageModel<PhysicalServerOptionViewModel>>(options);
            return Ok(viewModel);
        }

        /// <summary>
        /// Создать опцию для физического сервера
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]PhysicalServerOptionViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var param = new PhysicalServerOptionsParams
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Type = model.Type
            };
            var server = _serverService.CreateOrUpdateOption(param);

            return Ok(new { id = server.Id });
        }

        ///// <summary>
        ///// Создать опции для физического сервера
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult Post([FromBody]IEnumerable<PhysicalServerOptionViewModel> options)
        //{
        //    if (!this.ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var param = options.Select(opt => new PhysicalServerOptionsParams
        //    {
        //        Name = opt.Name,
        //        Description = opt.Description,
        //        Price = opt.Price,
        //        Type = opt.Type
        //    });
        //    _serverService.CreateOrUpdateOptions(param);

        //    return Ok();
        //}

        /// <summary>
        /// Добавить доступные опции для физического сервера
        /// </summary>
        /// <param name="serverId"></param>
        /// <param name="options"></param>
        /// <param name="replaceAll"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult ChangeOptionsAviable([FromBody]PhysicalServerChangeOptionsAviable model)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Guid guid;
            if (!Guid.TryParse(model.ServerId, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            var parameters = new PhysicalServerOptionsAviableParams { ServerId = guid, ReplaceAll = model.ReplaceAll };
            var optionsnAviable = new List<OptionAviable>();
            foreach (var opt in model.Options)
            {
                Guid optguid;
                if (!Guid.TryParse(opt.Id, out optguid))
                {
                    ModelState.AddModelError("id", "Invalid Guid format");
                    return BadRequest(ModelState);
                }
                var ids = new OptionAviable { OptionId = optguid, IsDefault = opt.IsDefault };
                optionsnAviable.Add(ids);
            }
            parameters.Options = optionsnAviable;
            _serverService.UpdateOptionsAviable(parameters);

            return Ok();
        }

        /// <summary>
        /// Удалить опцию физического сервера
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeletePhysicalServerOption(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                ModelState.AddModelError("id", "Invalid Guid format");
                return BadRequest(ModelState);
            }
            _serverService.DeletePhysicalServerOption(guid);

            return Ok();
        }
    }
}
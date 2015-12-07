using Crytex.Service.IService;
using Crytex.Web.Models.JsonModels;
using System;
using System.Web.Http;

namespace Crytex.Web.Areas.Admin.Controllers
{
    public class AdminVmBackupController : AdminCrytexController
    {
        private IVmBackupService _backupService;

        public AdminVmBackupController(IVmBackupService backupService)
        {
            this._backupService = backupService;
        }

        public IHttpActionResult Get(string id)
        {
            Guid guid;
            if (!Guid.TryParse(id, out guid))
            {
                this.ModelState.AddModelError("id", "Invalid Guid format");
                return this.BadRequest(ModelState);
            }

            var backup = this._backupService.GetById(guid);
            var model = AutoMapper.Mapper.Map<VmBackupViewModel>(backup);

            return this.Ok(model);
        }

        public IHttpActionResult Get(int pageNumber, int pageSize, DateTime? from = null, DateTime? to = null)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("PageNumber and PageSize must be grater than 1");

            var pagedList = this._backupService.GetPage(pageNumber, pageSize, from, to);
            var pageModel = AutoMapper.Mapper.Map<PageModel<VmBackupViewModel>>(pagedList);

            return this.Ok(pageModel);
        }
    }
}

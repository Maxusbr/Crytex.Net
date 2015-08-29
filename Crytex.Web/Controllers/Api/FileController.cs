using Crytex.Service.IService;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Crytex.Web.Models.JsonModels;

namespace Crytex.Web.Controllers.Api
{
    public class FileController : CrytexApiController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        [HttpPost]
        public HttpResponseMessage Create(FileDescriptorViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var newDescriptor = this._fileService.CreateFileDescriptor(model.Name, model.Type.Value, model.Path);
                return Request.CreateResponse(HttpStatusCode.Created, new { id = newDescriptor.Id });
            }

            return this.Request.CreateErrorResponse(HttpStatusCode.BadRequest, this.ModelState);
        }
    }
}

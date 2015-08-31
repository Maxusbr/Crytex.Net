using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Project.Web.Models.JsonModels;
using System.Web;
using System.Threading.Tasks;
using System.IO;
using Project.Model.Models;

namespace Project.Web.Controllers.Api
{
    public class FileController : CrytexApiController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Upload()
        {
            // Check if the request contains multipart/form-data.
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            // Read the form data.
            await Request.Content.ReadAsMultipartAsync(provider);
            if (provider.FormData.Get("fileType") == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "FileType is required");
            }

            FileType fileType = (FileType)Enum.Parse(typeof(FileType), provider.FormData.Get("fileType"));
            MultipartFileData file = provider.FileData.SingleOrDefault();
            if (file != null)
            {
                using (var stream = new FileStream(file.LocalFileName, FileMode.Open))
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var descriptor = this._fileService.SaveFile(stream, fileName, "C:/testTemp/", fileType);
                    
                    return Request.CreateResponse(HttpStatusCode.OK, new { id = descriptor.Id });
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No files to upload");
            }
        }
    }
}

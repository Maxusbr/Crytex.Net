using Crytex.Service.IService;
using System.Threading.Tasks;
using System.IO;
using Crytex.Model.Models;
using Crytex.Web.Models.JsonModels;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;

namespace Crytex.Web.Controllers.Api
{
    public class FileController : CrytexApiController
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        public async Task<IHttpActionResult> Post()
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
                return BadRequest("FileType is required");
            }

            FileType fileType = (FileType)Enum.Parse(typeof(FileType), provider.FormData.Get("fileType"));
            
            MultipartFileData file = provider.FileData.SingleOrDefault();
            if (file != null)
            {
                FileDescriptor descriptor;
                using (var stream = File.OpenRead(file.LocalFileName))
                {
                    var fileName = file.Headers.ContentDisposition.FileName.Trim('\"');
                    var savePath = HttpContext.Current.Server.MapPath(this.GetSavePath(fileType));
                    if (fileType == FileType.Image)
                    {
                        int smallSize = this.CrytexContext.ServerConfig.GetSmallImageSize();
                        int bigSize = this.CrytexContext.ServerConfig.GetBigImageSize();
                        descriptor = this._fileService.SaveImageFile(stream, fileName, savePath, smallSize, bigSize);
                    }
                    else
                    {
                        descriptor = this._fileService.SaveFile(stream, fileName, savePath, fileType);
                    }
                }

                File.Delete(file.LocalFileName);

                return Ok(new { id = descriptor.Id });
            }

            return BadRequest("No files to upload");

        }

        private string GetSavePath(FileType fType)
        {
            string path = null;

            switch (fType)
            {
                case FileType.Document:
                    path = this.CrytexContext.ServerConfig.GetDocumentFileSavePath();
                    break;
                case FileType.Image:
                    path = this.CrytexContext.ServerConfig.GetImageFileSavePath();
                    break;
                case FileType.Loader:
                    path = this.CrytexContext.ServerConfig.GetLoaderFileSavePath();
                    break;
            }

            return path;
        }
    }
}

using Crytex.Service.IService;
using System.Threading.Tasks;
using System.IO;
using Crytex.Model.Models;
using Crytex.Web.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Http.Description;
using Crytex.Core.Service;
using Microsoft.Practices.Unity;

namespace Crytex.Web.Areas.Admin
{
    public class AdminFileController : AdminCrytexController
    {
        private readonly IFileService _fileService;
        [Dependency]
        public IServerConfig _serverConfig { get; set; }

        public AdminFileController(IFileService fileService)
        {
            this._fileService = fileService;
        }

        /// <summary>
        /// Получение списка файлов
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(FileDescriptorViewModel))]
        public IHttpActionResult Get()
        {
            var files = _fileService.GetAll();
            var viewFiles = AutoMapper.Mapper.Map<List<FileDescriptorViewModel>>(files);
            return Ok(viewFiles);
        }

        /// <summary>
        /// Создание нового файла
        /// </summary>
        /// <returns></returns>
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
                    var savePath = HttpContext.Current.Server.MapPath(this.GetPath(fileType));
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

                String path = String.Empty;

                if (fileType == FileType.Image)
                {
                    path = _serverConfig.GetImageFileSavePath() + "/small_" + descriptor.Path;
                }
                else
                {
                    path = descriptor.Path;
                }

                return Ok(new { id = descriptor.Id, path = path });
            }

            return BadRequest("No files to upload");

        }

        /// <summary>
        /// Удаление файла по id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IHttpActionResult Delete(int id)
        {
            var file =_fileService.GetById(id);
            var deletePath = HttpContext.Current.Server.MapPath(this.GetPath(file.Type));
            _fileService.RemoveFile(file, deletePath);
            return Ok();
        }

        private string GetPath(FileType fType)
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

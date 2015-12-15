using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using Crytex.Service.IService;
using System.Collections.Generic;

namespace Crytex.Service.Service
{
    public class ServerTemplateService : IServerTemplateService
    {
        private readonly IServerTemplateRepository _serverTemplateRepo;
        private readonly IOperatingSystemRepository _operatingSystemRepo;
        private readonly IUnitOfWork _unitOfWork;

        public ServerTemplateService(IUnitOfWork unitOfWork, IOperatingSystemRepository osRepo,
            IServerTemplateRepository templateRepo)
        {
            this._unitOfWork = unitOfWork;
            this._operatingSystemRepo = osRepo;
            this._serverTemplateRepo = templateRepo;
        }

        public ServerTemplate CreateTemplate(ServerTemplate newTemplate)
        {
            this._serverTemplateRepo.Add(newTemplate);
            this._unitOfWork.Commit();

            return newTemplate;
        }


        public IEnumerable<ServerTemplate> GeAllForUser(string userId)
        {
            var servers = this._serverTemplateRepo.GetMany(t => t.UserId == userId);

            return servers;
        }

        public IEnumerable<ServerTemplate> GetSystemTemplates()
        {
            var servers = this._serverTemplateRepo.GetMany(t => t.UserId == null);

            return servers;
        }

        public ServerTemplate GetById(int id)
        {
            var template = this._serverTemplateRepo.Get(x => x.Id == id, x => x.OperatingSystem);

            if (template == null)
            {
                throw new InvalidIdentifierException(string.Format("ServerTemplate with Id={0} doesn't exists", id));
            }

            return template;
        }

        public void Update(int id, ServerTemplate updatedTemplate)
        {
            var template = this._serverTemplateRepo.GetById(id);

            if (template == null)
            {
                throw new InvalidIdentifierException(string.Format("ServerTemplate width Id={0} doesn't exists", id));
            }

            template.Description = updatedTemplate.Description;
            template.ImageFileId = updatedTemplate.ImageFileId;
            template.OperatingSystemId = updatedTemplate.OperatingSystemId;
            template.RamCount = updatedTemplate.RamCount;
            template.HardDriveSize = updatedTemplate.HardDriveSize;
            template.CoreCount = updatedTemplate.CoreCount;
            template.Name = updatedTemplate.Name;

            this._serverTemplateRepo.Update(template);
            this._unitOfWork.Commit();
        }

        public void DeleteById(int id)
        {
            var template = this._serverTemplateRepo.GetById(id);

            if (template == null)
            {
                throw new InvalidIdentifierException(string.Format("OperatingSystem with Id={0} doesn't exists", id));
            }

            this._serverTemplateRepo.Delete(template);
            this._unitOfWork.Commit();
        }
    }
}

using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Model.Exceptions;
using Project.Model.Models;
using Project.Service.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Service
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

        public ServerTemplate GeById(int id)
        {
            var template = this._serverTemplateRepo.GetById(id);

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
            template.MinRamCount = updatedTemplate.MinRamCount;
            template.MinHardDriveSize = updatedTemplate.MinHardDriveSize;
            template.MinCoreCount = updatedTemplate.MinCoreCount;

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

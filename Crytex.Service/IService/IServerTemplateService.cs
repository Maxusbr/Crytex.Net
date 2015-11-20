using Crytex.Model.Models;
using System.Collections.Generic;

namespace Crytex.Service.IService
{
    public interface IServerTemplateService
    {
        ServerTemplate CreateTemplate(ServerTemplate newTemplate);

        IEnumerable<ServerTemplate> GeAllForUser(string userId);


        IEnumerable<ServerTemplate> GetSystemTemplates();
        ServerTemplate GetById(int id);

        void Update(int id, ServerTemplate updatedTemplate);

        void DeleteById(int id);
    }
}

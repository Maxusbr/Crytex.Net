using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Service.IService
{
    public interface IServerTemplateService
    {
        ServerTemplate CreateTemplate(ServerTemplate newTemplate);

        IEnumerable<ServerTemplate> GeAllForUser(string userId);

        ServerTemplate GeById(int id);

        void Update(int id, ServerTemplate updatedTemplate);

        void DeleteById(int id);
    }
}

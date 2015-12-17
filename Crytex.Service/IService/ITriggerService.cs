using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Service.IService
{
    public interface ITriggerService
    {
        Trigger CreateTrigger(Trigger trigger);

        void UpdateTrigger(Trigger trigger);

        void DeleteTriggerById(Guid id);

        Trigger GetTriggerById(Guid id);

        IPagedList<Trigger> GetPage(int pageNumber, int pageSize, string userId = null);
        void InitStandartTriggersForUser(string userId);
        List<Trigger> GetUserTriggers(string userId, TriggerType? type = null);
        Trigger GetUserTrigger(string userId, TriggerType type, double value);

    }
}

using Crytex.Model.Models;
using System.Collections.Generic;
using Crytex.Service.Model;
using PagedList;

namespace Crytex.Service.IService
{
    public interface IStatisticService
    {
        IEnumerable<Statistic> GetAllStatistics();
        IPagedList<Statistic> GetAllPageStatistics(int pageNumber, int pageSize, StatisticType? type = null);
        Statistic GetStatisticById(int id);
        Statistic CreateStatistic(Statistic newStatistic);
        void UpdateStatistic(Statistic updatedStatistic);
        void DeleteStatisticById(int id);
        void CalculateStatistic(TypeStatistic type);
    }
}

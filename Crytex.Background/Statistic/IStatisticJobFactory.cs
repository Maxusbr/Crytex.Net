using Crytex.Model.Models;

namespace Crytex.Background.Statistic
{
    public interface IStatisticJobFactory
    {
        void CreateStatisticJob(TypeStatistic typeStatistic);
    }
}
using System;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Data.IRepository
{
    public interface IHelpDeskRequestRepository : IRepository<HelpDeskRequest>
    {
        IPagedList<HelpDeskRequest> GetPage<TOrder, TSecondOrder>(Page page, Expression<Func<HelpDeskRequest, bool>> where,
            Expression<Func<HelpDeskRequest, TOrder>> order,
            Expression<Func<HelpDeskRequest, TSecondOrder>> secondOreder);
    }
}

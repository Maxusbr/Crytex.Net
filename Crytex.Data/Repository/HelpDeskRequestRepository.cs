using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using PagedList;

namespace Crytex.Data.Repository
{
    public class HelpDeskRequestRepository : RepositoryBase<HelpDeskRequest>, IHelpDeskRequestRepository
    {
        public HelpDeskRequestRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }

        public  IPagedList<HelpDeskRequest> GetPage<TOrder, TSecondOrder>(Page page, Expression<Func<HelpDeskRequest, bool>> where, Expression<Func<HelpDeskRequest, TOrder>> order, Expression<Func<HelpDeskRequest, TSecondOrder>> secondOrder)
        {
            var query = this.DataContext.HelpDeskRequests
                .Where(where)
                .OrderBy(order);
            if (secondOrder != null) query = query.ThenByDescending(secondOrder);
            var pageQuery = query.GetPage(page);

            var finalQuery = this.AppendIncludesToRequest(pageQuery);

            var results = finalQuery.ToList();
            var total = results.Count();

            return new StaticPagedList<HelpDeskRequest>(results, page.PageNumber, page.PageSize, total);
        }

        private IQueryable<HelpDeskRequest> AppendIncludesToRequest(IQueryable<HelpDeskRequest> query)
        {
            query = query
                .Include(vm => vm.User);
            return query;
        }
    }
}

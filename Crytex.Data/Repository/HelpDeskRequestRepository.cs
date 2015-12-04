using System;
using System.Collections.Generic;
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

        public  IPagedList<HelpDeskRequest> GetPage<TOrder, TSecondOrder>(Page page, Expression<Func<HelpDeskRequest, bool>> where, Expression<Func<HelpDeskRequest, TOrder>> order, Expression<Func<HelpDeskRequest, TSecondOrder>> secondOrder, params Expression<Func<HelpDeskRequest, object>>[] includes)
        {
            var query = this.DataContext.HelpDeskRequests
                .Where(where)
                .OrderBy(order);
            if (secondOrder != null) query = query.ThenByDescending(secondOrder);
            var pageQuery = query.GetPage(page);

            var finalQuery = this.AppendIncludes(pageQuery, includes);

            var results = finalQuery.ToList();
            var total = this.DataContext.HelpDeskRequests.Count(where);

            return new StaticPagedList<HelpDeskRequest>(results, page.PageNumber, page.PageSize, total);
        }

        private IQueryable<HelpDeskRequest> AppendIncludes(IQueryable<HelpDeskRequest> query, IEnumerable<Expression<Func<HelpDeskRequest, object>>> includes)
        {
            foreach (var inc in includes)
            {
                query = query.Include(inc);
            }

            return query;
        }
    }
}

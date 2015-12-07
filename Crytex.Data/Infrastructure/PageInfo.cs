using System.Linq;

namespace Crytex.Data.Infrastructure
{
    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public PageInfo()
        {
            PageNumber = 1;
            PageSize = 10;
        }
     
        public PageInfo(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        public int Skip
        {
            get { return (PageNumber - 1) * PageSize; }
        }
    }

    public static class PagingExtensions
    {
        /// <summary>
        /// Extend IQueryable to simplify access to skip and take methods 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="page"></param>
        /// <returns>IQueryable with Skip and Take having been performed</returns>
        public static IQueryable<T> GetPage<T>(this IQueryable<T> queryable, PageInfo page)
        {
            return queryable.Skip(page.Skip).Take(page.PageSize);
        }
    }
}
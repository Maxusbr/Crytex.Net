using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using System.Linq.Expressions;
using PagedList;
using System.Data.Entity;

namespace Crytex.Data.Repository
{
   public  class UserVmRepository : RepositoryBase<UserVm>, IUserVmRepository
    {
        public UserVmRepository(IDatabaseFactory databaseFactory)
            : base(databaseFactory)
        {
        }
        //TODO: разобраться в ошибке, была следующая строка:
        //public override IPagedList<UserVm> GetPage<TOrder>(Page page, Expression<Func<UserVm, bool>> where, Expression<Func<UserVm, TOrder>> order)
        public IPagedList<UserVm> GetPage<TOrder>(Page page, Expression<Func<UserVm, bool>> where, Expression<Func<UserVm, TOrder>> order)
        {
            var pageQuery = this.DataContext.UserVms
                .Where(where)
                .OrderBy(order)
                .GetPage(page);
            var finalQuery = this.AppendIncludesToVmQuey(pageQuery);
            var vmsResultList = finalQuery.ToList();
                

            var total = vmsResultList.Count();
            var vmsPagedList = new StaticPagedList<UserVm>(vmsResultList, page.PageNumber, page.PageSize, total);

            return vmsPagedList;
        }

        public override UserVm GetById(Guid guid)
        {
            var userVmQuery = this.DataContext.UserVms.Where(vm => vm.Id == guid);
            var finalQuery = this.AppendIncludesToVmQuey(userVmQuery);
            var result = finalQuery.SingleOrDefault();

            return result;
        }

        private IQueryable<UserVm> AppendIncludesToVmQuey(IQueryable<UserVm> query)
        {
            query = query
                .Include(vm => vm.User)
                .Include(vm => vm.ServerTemplate)
                .Include(vm => vm.ServerTemplate.OperatingSystem)
                .Include(vm => vm.ServerTemplate.OperatingSystem.ImageFileDescriptor);
            
            return query;
        }
    }
}

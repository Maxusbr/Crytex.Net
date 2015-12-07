using Crytex.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Model.Models;
using System.Linq.Expressions;
using PagedList;

namespace Crytex.Test.FakeImplementations
{

    internal class FakeFileDescriptorRepo : IFileDescriptorRepository
    {
        public IList<Model.Models.FileDescriptor> _descriptorsStorage = new List<Model.Models.FileDescriptor>();

        public void Add(Model.Models.FileDescriptor entity)
        {
            this._descriptorsStorage.Add(entity);
        }

        public void Update(Model.Models.FileDescriptor entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Model.Models.FileDescriptor entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Model.Models.FileDescriptor GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Model.Models.FileDescriptor GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Model.Models.FileDescriptor GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Model.Models.FileDescriptor Get(System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, bool>> where, params System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Model.Models.FileDescriptor> GetAll(params System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Model.Models.FileDescriptor> GetMany(System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, bool>> where, params System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public PagedList.IPagedList<Model.Models.FileDescriptor> GetPage<TOrder>(Data.Infrastructure.PageInfo page, System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, bool>> where, System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, TOrder>> order, params System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        List<FileDescriptor> IRepository<FileDescriptor>.GetAll(params Expression<Func<FileDescriptor, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        List<FileDescriptor> IRepository<FileDescriptor>.GetMany(Expression<Func<FileDescriptor, bool>> where, params Expression<Func<FileDescriptor, object>>[] includes)
        {
            throw new NotImplementedException();
        }

        public IPagedList<FileDescriptor> GetPage<TOrder>(PageInfo page, Expression<Func<FileDescriptor, bool>> where, Expression<Func<FileDescriptor, TOrder>> order, bool reverse = false, params Expression<Func<FileDescriptor, object>>[] includes)
        {
            throw new NotImplementedException();
        }
    }
}

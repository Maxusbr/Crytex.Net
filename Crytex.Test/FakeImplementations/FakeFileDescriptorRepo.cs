using Crytex.Data.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Model.Models.FileDescriptor Get(System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, bool>> where)
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<Model.Models.FileDescriptor> GetAll()
        {
            throw new NotImplementedException();
        }

        public System.Collections.Generic.IEnumerable<Model.Models.FileDescriptor> GetMany(System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, bool>> where)
        {
            throw new NotImplementedException();
        }

        public PagedList.IPagedList<Model.Models.FileDescriptor> GetPage<TOrder>(Data.Infrastructure.Page page, System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, bool>> where, System.Linq.Expressions.Expression<Func<Model.Models.FileDescriptor, TOrder>> order)
        {
            throw new NotImplementedException();
        }
    }
}

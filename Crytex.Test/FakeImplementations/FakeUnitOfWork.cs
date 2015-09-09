using Crytex.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Test.FakeImplementations
{
    internal class FakeUnitOfWork : IUnitOfWork
    {
        public void Commit()
        {
        }

        public void Rollback()
        {
        }
    }
}

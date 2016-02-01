using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Virtualization.Base;

namespace Crytex.Virtualization.ProviderFactory
{
    public class FakeProviderFactory: IProviderFactory
    {
        public IProviderVM GetProviderVm(ProviderVirtualization provider)
        {
            throw new NotImplementedException();
        }
    }
}

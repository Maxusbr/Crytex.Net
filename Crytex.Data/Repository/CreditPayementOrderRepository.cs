using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crytex.Data.Repository
{
    public class CreditPayementOrderRepository : RepositoryBase<CreditPaymentOrder>, ICreditPaymentOrderRepository
    {
        public CreditPayementOrderRepository(DatabaseFactory dbFactory) : base(dbFactory) { }
    }
}

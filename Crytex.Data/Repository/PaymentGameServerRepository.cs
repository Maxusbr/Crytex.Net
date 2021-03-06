﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Data.Infrastructure;
using Crytex.Data.IRepository;
using Crytex.Model.Models.Biling;

namespace Crytex.Data.Repository
{
    public class PaymentGameServerRepository : RepositoryBase<PaymentGameServer>, IPaymentGameServerRepository
    {
        public PaymentGameServerRepository(IDatabaseFactory databaseFactory) : base(databaseFactory) { }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Project.Data.Infrastructure;
using Project.Data.IRepository;
using Project.Data.Repository;
using Project.Service.Service;
using Sample.Service.IService;

namespace Project.Service
{
    public class UnityConfig
    {
        public static void Register(IUnityContainer container)
        {
            container.RegisterType<IMessageRepository, MessageRepository>();
            container.RegisterType<IUnitOfWork, UnitOfWork>(new PerThreadLifetimeManager());
            container.RegisterType<IDatabaseFactory, DatabaseFactory>(new PerThreadLifetimeManager());

            container.RegisterType<ISender, SenderWcf>();
            container.RegisterType<IMessageService, MessageService>();

        }
    }
}

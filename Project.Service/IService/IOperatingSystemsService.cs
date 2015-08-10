﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OperatingSystem = Project.Model.Models.OperatingSystem;

namespace Project.Service.IService
{
    public interface IOperatingSystemsService
    {
        OperatingSystem CreateOperatingSystem(OperatingSystem newOS);

        OperatingSystem GeById(int id);

        IEnumerable<OperatingSystem> GetAll();

        void DeleteById(int id);

        void Update(int id, OperatingSystem updatedOs);
    }
}

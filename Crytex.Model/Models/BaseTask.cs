﻿using System;

namespace Crytex.Model.Models
{
    public class BaseTask: BaseEntity
    {
        public StatusTask StatusTask { get; set; }
        public String UserId { get; set; }
        public TypeVirtualization Virtualization { get; set; }
    }

    public enum TypeVirtualization
    {
        HyperV = 0,
        WmWare = 1
    }

    public enum StatusTask
    {
        Start = 0,
        Pending = 1,
        Processing = 2,
        End = 3
    }
}
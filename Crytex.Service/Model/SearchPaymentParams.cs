﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Enums;

namespace Crytex.Service.Model
{
    public class SearchPaymentParams
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateType? DateType { get; set; }
        public bool? Success { get; set; }
        public string UserId { get; set; }
    }

    public enum DateType
    {
        StartTransaction,
        EndTramsaction
    }
}
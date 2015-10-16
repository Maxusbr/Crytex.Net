﻿using Crytex.Model.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Crytex.Web.Models.JsonModels
{
    public class HelpDeskRequestViewModel
    {
        [Required]
        public string Summary { get; set; }

        [Required]
        public string Details { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime CreationDate { get; set; }

        public int Id { get; set; }

        public string UserId { get; set; }
    }
}
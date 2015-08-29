﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crytex.Model.Models
{
    public class OperatingSystem : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Int32 ImageFileId { get; set; }
        public Int32 LoaderFileId { get; set; }

        [ForeignKey("ImageFileId")]
        public FileDescriptor ImageFileDescriptor { get; set; }
        [ForeignKey("LoaderFileId")]
        public FileDescriptor LoaderFileDescriptor { get; set; }
    }
}
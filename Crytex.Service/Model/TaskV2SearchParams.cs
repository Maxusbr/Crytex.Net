using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crytex.Model.Models;

namespace Crytex.Service.Model
{
    public class TaskV2SearchParams
    {
        public Guid? ResourceId { get; set; }
        public TypeTask? TypeTask { get; set; }
        public StatusTask[] StatusTasks { get; set; }
        public TypeVirtualization? Virtualization { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TypeDate? TypeDate { get; set; }
        public string UserId { get; set; }
    }

    public enum TypeDate
    {
        StartedAt,
        CompletedAt
    }
}

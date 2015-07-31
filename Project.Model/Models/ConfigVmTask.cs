using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Model.Models
{
    public class ConfigTask :BaseTask
    {
        public Int32 Cpu;
        public Int32 Ram;
        public Int32 Hdd;
        public String Name;
    }
}

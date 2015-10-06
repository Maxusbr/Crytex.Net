using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Crytex.Model.Models
{
    public class TaskV2
    {
        public Guid Id { get; set; }
        public Guid? ResourceId { get; set; }
        public TypeTask TypeTask { get; set; }
        public StatusTask StatusTask { get; set; }
        public ResourceType ResourceType { get; set; }
        public string Options { get; set; }
        public string UserId { get; set; }
        public string ErrorMessage { get; set; }
        public TypeVirtualization Virtualization { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime CompletedAt { get; set; }

        
        public void SaveOptions<T>(T value) where T: BaseOptions
        {
            Options = JsonConvert.SerializeObject(value ?? new BaseOptions());
        }

        
        public T GetOptions<T>() where T : BaseOptions
        {
            return JsonConvert.DeserializeObject<T>(Options); 
        }

    }
    public enum TypeTask 
    {
        CreateVm,
        UpdateVm,
        ChangeStatus,
        RemoveVm
    }

    [Serializable]
    public class BaseOptions
    {

    }

    [Serializable]
    public class ConfigVmOptions : BaseOptions
    {
        public Int32 Cpu { get; set; }
        public Int32 Ram { get; set; }
        public Int32 Hdd { get; set; }
        public String Name { get; set; }
    }

    [Serializable]
    public class CreateVmOptions : ConfigVmOptions
    {
        public Int32 ServerTemplateId { get; set; }
    }

    [Serializable]
    public class UpdateVmOptions : ConfigVmOptions
    {
        public Guid VmId { get; set; }
    }

    [Serializable]
    public class ChangeStatusOptions : BaseOptions
    {
        public TypeChangeStatus TypeChangeStatus { get; set; }
        public Guid VmId { get; set; }
    }

    public enum TypeChangeStatus {
        Start,
        Stop,
        Reload,
        PowerOf
    }

    public enum ResourceType
    {
        Vm = 0
    }
}



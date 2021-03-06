﻿using System;
using Newtonsoft.Json;

namespace Crytex.Model.Models
{
    public class TaskV2
    {
        public Guid Id { get; set; }
        public ResourceType ResourceType { get; set; }
        public Guid? ResourceId { get; set; }
        public TypeTask TypeTask { get; set; }
        public StatusTask StatusTask { get; set; }
        public string Options { get; set; }
        public string UserId { get; set; }
        public string ErrorMessage { get; set; }
        public TypeVirtualization Virtualization { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }


        public void SaveOptions<T>(T value) where T : BaseOptions
        {
            Options = JsonConvert.SerializeObject(value ?? new BaseOptions());
        }


        public T GetOptions<T>() where T : BaseOptions
        {
            return JsonConvert.DeserializeObject<T>(Options);
        }

    }

    public enum TypeVirtualization
    {
        HyperV = 0,
        VmWare = 1
    }


    public enum StatusTask
    {
        Start = 0,
        Pending = 1,
        Processing = 2,
        End = 3,
        EndWithErrors = 4,
        Queued = 5
    }

    public enum TypeTask
    {
        CreateVm = 0,
        UpdateVm = 1,
        ChangeStatus = 2,
        RemoveVm = 3,
        Backup = 4,
        DeleteBackup = 5,
        CreateSnapshot = 6,
        DeleteSnapshot = 7,
        LoadSnapshot = 8,
        CreateWebHosting = 9,
        StartWebApp = 10,
        StopWebApp = 11,
        RestartWebApp = 12,
        DisableWebHosting = 13,
        DeleteHosting = 14,
        CreateGameServer = 15,
        DeleteGameServer = 16,
        GameServerChangeStatus = 17,
        UpdateGameServer = 18,



        Test = 99,
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
        public Int32 HddGB { get; set; }
        // Пользовательское имя виртауальной машины. Используется для отображения на UI
        public String Name { get; set; }
    }

    [Serializable]
    public class CreateVmOptions : ConfigVmOptions
    {
        public int OperatingSystemId { get; set; }
        public Guid UserVmId { get; set; }
    }

    [Serializable]
    public class UpdateVmOptions : ConfigVmOptions
    {
        public Guid VmId { get; set; }
    }

    [Serializable]
    public class RemoveVmOptions : BaseOptions
    {
        public Guid VmId { get; set; }
    }

    [Serializable]
    public class ChangeStatusOptions : BaseOptions
    {
        public TypeChangeStatus TypeChangeStatus { get; set; }
        public Guid VmId { get; set; }
    }

    [Serializable]
    public class BackupOptions : BaseOptions
    {
        public string BackupName { get; set; }
        public Guid VmId { get; set; }
        public Guid VmBackupId { get; set; }
    }

    [Serializable]
    public class CreateSnapshotOptions : BaseOptions
    {
        public Guid SnapshotId { get; set; }
        public Guid VmId { get; set; }
    }

    [Serializable]
    public class DeleteSnapshotOptions : BaseOptions
    {
        public Guid SnapshotId { get; set; }
        public bool DeleteWithChildrens { get; set; }
        public Guid VmId { get; set; }
    }

    [Serializable]
    public class LoadSnapshotOptions : BaseOptions
    {
        public Guid SnapshotId { get; set; }
        public Guid VmId { get; set; }
    }

    [Serializable]
    public class DeleteBackupOptions : BaseOptions
    {
        public Guid VmBackupId { get; set; }
    }

    [Serializable]
    public class CreateWebHostingOptions : BaseOptions
    {
    }

    [Serializable]
    public class DisableWebHostingOptions : BaseOptions
    {
    }

    [Serializable]
    public class DeleteWebHostingOptions : BaseOptions
    {
    }

    [Serializable]
    public class WebApplicationTaskOptions : BaseOptions
    {
        public Guid HostedWedApplicationId { get; set; }
    }

    public class BaseGameServerOptions : BaseOptions
    {
        public Guid GameServerId { get; set; }
    }

    [Serializable]
    public class CreateGameServerOptions : BaseGameServerOptions
    {
        public int SlotCount { get; set; }
        public int GameServerFirstPortInRange { get; set; }
    }

    [Serializable]
    public class DeleteGameServerOptions : BaseGameServerOptions
    {
        
    }

    [Serializable]
    public class ChangeGameServerStatusOptions : BaseGameServerOptions
    {
        public string GameServerPassword { get; set; }
        public TypeChangeStatus TypeChangeStatus { get; set; }
        public int GameServerPort { get; set; }
    }

    [Serializable]
    public class UpdateGameServerOptions : BaseGameServerOptions
    {
        public int? SlotCount { get; set; }
    }

    public enum TypeChangeStatus {
        Start,
        Stop,
        Reload,
        PowerOff
    }

    public enum ResourceType
    {
        SubscriptionVm = 0,
        WebHosting = 1,
        WebApp = 2,
        GameServer = 3
    }
}



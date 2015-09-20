﻿using Crytex.Model.Models.Notifications;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using Crytex.Model.Models;
using Crytex.Notification.Service;
using Crytex.Service.IService;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR.Hubs;


namespace Crytex.Notification
{
    public class MonitorHub : CrytexHub
    {
        private readonly static Dictionary<Guid, List<string>> VmDictionary =
            new Dictionary<Guid, List<string>>();
        private readonly IUserVmService _userVmService;

        public MonitorHub(IUserVmService userVmService)
        {
            this._userVmService = userVmService;
        }

        public void Sybscribe(string vmId)
        {
            Guid VmId;
            if (!Guid.TryParse(vmId, out VmId))
            {
                return;
            }
            UserVm VM;
            if (!((VM = _userVmService.GetVmById(VmId)).UserId == GetUserId() ||
                this.IsCurrentUserAdmin() || this.IsCurrentUserSupport()))
            {
                return;
            }

            AddVmConnection(VM.Id, Context.ConnectionId);
        }

        public List<Guid> GetVMs()
        {
            List<Guid> VmList = VmDictionary.Keys.ToList();
            return VmList;
        }

        public void SendVmMessage(Guid VmId,StateMachine stateMachine)
        {
            foreach (string VmConnectionId in GetVMConnections(VmId)) 
            {
                Clients.Client(VmConnectionId).RecieveVmMessage(stateMachine);
            }
        }

        private void AddVmConnection(Guid key, string connectionId)
        {
            lock (VmDictionary)
            {
                List<string> VmConnections;
                if (!VmDictionary.TryGetValue(key, out VmConnections))
                {
                    VmConnections = new List<string>();

                    VmDictionary.Add(key, VmConnections);
                }

                lock (VmConnections)
                {
                    VmConnections.Add(connectionId);
                }
            }
        }

        public void RemoveVmConnection(Guid key, string connectionId)
        {
            lock (VmDictionary)
            {
                List<string> VmConnections;
                if (!VmDictionary.TryGetValue(key, out VmConnections))
                {
                    return;
                }

                lock (VmConnections)
                {
                    try
                    {
                        var indexConnectionInList = VmConnections.FindIndex(c=>c.Contains(connectionId));
                        VmConnections.RemoveAt(indexConnectionInList);
                    }
                    catch (ArgumentNullException)
                    {
                        return;
                    }
                    if (VmConnections.Count == 0)
                    {
                        VmDictionary.Remove(key);
                    }
                }
            }
        }

        public List<string> GetVMConnections(Guid key)
        {
            List<string> VmConnections;
            if (VmDictionary.TryGetValue(key, out VmConnections))
            {
                return VmConnections;
            }

            return new List<string>();
        }

        public class StateMachine
        {
            public Int32 CpuLoad { get; set; }
            public Int32 RamLoad { get; set; }
            public DateTime Date { get; set; }
        }
    }


}
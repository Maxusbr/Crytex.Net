using Crytex.Model.Exceptions;
using Crytex.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VmWareRemote.Exceptions;
using VmWareRemote.Implementations;
using VmWareRemote.Interface;
using OperatingSystem = Crytex.Model.Models.OperatingSystem;

namespace Crytex.ExecutorTask.TaskHandler.VmWare
{
    public class VmWareControl : IVmWareControl
    {
        private IVmWareProvider _vmWareProvider;

        public VmWareControl(IVmWareProvider vmWareProvider)
        {
            this._vmWareProvider = vmWareProvider;
        }

        public CreateVmResult CreateVm(TaskV2 task, OperatingSystem os)
        {
            // Connect to server
            ConnectIfNotConnected();

            // Create new vm
            var createOptions = task.GetOptions<CreateVmOptions>();

            var machineGuid = createOptions.UserVmId;
            var machineName = machineGuid.ToString();

            var newPassword = System.Web.Security.Membership.GeneratePassword(6, 0);

            CreateVmResult result = new CreateVmResult();
            try
            {
                this._vmWareProvider.CloneVm(os.ServerTemplateName, machineName);
                this._vmWareProvider.ModifyMachine(machineName, createOptions.Cpu, createOptions.Ram, createOptions.HddGB);
                this._vmWareProvider.StartMachine(machineName, true);

                var oldPassword = os.DefaultAdminPassword;
                var adminUserName = os.DefaultAdminName;
                var scriptBuilder = new StringBuilder();
                switch (os.Family)
                {
                    case OperatingSystemFamily.Windows2012:
                        scriptBuilder.AppendLine("$Computername = $env:COMPUTERNAME");
                        scriptBuilder.AppendLine("$user = \"Administrator\"");
                        scriptBuilder.AppendLine("$user = [adsi]\"WinNT://$ComputerName/$user,user\"");
                        scriptBuilder.AppendLine("$pass = \""+newPassword+"\"");
                        scriptBuilder.AppendLine("$user.SetPassword($pass)");
                        this._vmWareProvider.RunPowerShellScript(machineName, adminUserName, oldPassword, scriptBuilder.ToString());
                        break;
                    case OperatingSystemFamily.Ubuntu:
                        scriptBuilder.AppendFormat("echo -e \"{0}\\n{1}\\n{1}\" | passwd", oldPassword, newPassword);
                        this._vmWareProvider.RunLinuxShellScript(machineName, adminUserName, oldPassword, scriptBuilder.ToString());
                        break;
                }
                result.IpAddresses = this._vmWareProvider.GetMachineByName(machineName).NetworkInfo;
                
            }
            catch (ApplicationException ex)
            {
                throw new CreateVmException(ex.Message, ex);
            }

            result.MachineGuid = machineGuid;
            result.GuestOsAdminPassword = newPassword;

            return result;
        }


        public void UpdateVm(TaskV2 updateVmTask)
        {
            // Connect to server
            ConnectIfNotConnected();

            try
            {
                // Get update options
                var updateOptions = updateVmTask.GetOptions<UpdateVmOptions>();

                // Get server machine-name from machine Id
                var machineName = updateVmTask.ResourceId.ToString();

                // Calculate Ram size in megabytes
                var ramMB = updateOptions.Ram * 1024;

                // Update vm conf by calling provider's ModifyMachine
                this._vmWareProvider.ModifyMachine(machineName, updateOptions.Cpu, ramMB);
            }
            // Handle invalid name exceptions
            catch (InvalidNameException e)
            {
                throw new InvalidIdentifierException("Invalid name. See inner exception", e);
            }
        }

        public void StartVm(string machineName)
        {
            // Connect to server
            ConnectIfNotConnected();

            try
            {
                this._vmWareProvider.StartMachine(machineName, false);
            }
            // Handle invalid name exceptions
            catch (InvalidNameException e)
            {
                throw new InvalidIdentifierException("Invalid name. See inner exception", e);
            }
        }

        public void StopVm(string machineName)
        {
            // Connect to server
            ConnectIfNotConnected();

            try
            {
                this._vmWareProvider.StopMachine(machineName);
            }
            // Handle invalid name exceptions
            catch (InvalidNameException e)
            {
                throw new InvalidIdentifierException("Invalid name. See inner exception", e);
            }
        }

        public void RemoveVm(string machineName)
        {
            // Connect to server
            ConnectIfNotConnected();

            try
            {
                this._vmWareProvider.DeleteMachine(machineName);
            }
            // Handle invalid name exceptions
            catch (InvalidNameException e)
            {
                throw new InvalidIdentifierException("Invalid name. See inner exception", e);
            }
        }

        #region Private methods
        private void ConnectIfNotConnected()
        {
            if (this._vmWareProvider.ConnectionState != ConnectionState.Connected)
            {
                this._vmWareProvider.Connect();
            }
        }

        public Guid BackupVm(TaskV2 taskEntity)
        {
            throw new NotImplementedException();
        }
        #endregion // Private methods
    }
}

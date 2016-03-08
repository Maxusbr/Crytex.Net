namespace Crytex.Background.Monitor
{
    public interface IVmMonitor
    {
        VmState GetMachineState(string machineName);
    }
}

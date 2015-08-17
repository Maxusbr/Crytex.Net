using System;
using Crytex.ExecutorTask;
using Microsoft.Practices.Unity;

namespace Crytex.WCF.Receiver
{

    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "ControlService" в коде и файле конфигурации.
    public class ReceiverService : IReceiverService
    {
        private Executor _executor = UnityConfig.GetConfiguredContainer().Resolve<Executor>();
        
        public void CreateVm(int ID)
        {
            throw new NotImplementedException();
        }

        public void UpdateVm(int ID)
        {
            throw new NotImplementedException();
        }

        public void StandartVmTask(int ID)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Crytex.WCF.Receiver
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени интерфейса "IReceiverService" в коде и файле конфигурации.
    [ServiceContract]
    public interface IReceiverService
    {
        [OperationContract]
        void CreateVm(Int32 ID);

        [OperationContract]
        void UpdateVm(Int32 ID);


        [OperationContract]
        void StandartVmTask(Int32 ID);

    }
}

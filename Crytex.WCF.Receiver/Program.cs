using System;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;
using Crytex.ExecutorTask;

namespace Crytex.WCF.Receiver
{
    class Program
    {
      
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri(ConfigurationManager.AppSettings["host"]);
            using (ServiceHost host = new ServiceHost(typeof(ReceiverService), baseAddress))
            {
                // Enable metadata publishing.
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                host.Description.Behaviors.Add(smb);

                // Open the ServiceHost to start listening for messages. Since
                // no endpoints are explicitly configured, the runtime will create
                // one endpoint per base address for each service contract implemented
                // by the service.
                host.Open();

  
                Console.ReadLine();

                // Close the ServiceHost.
                host.Close();
            }
        }
    }
}

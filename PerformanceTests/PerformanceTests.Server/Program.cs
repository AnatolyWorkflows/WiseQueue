using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WiseQueue.Core.Common;
using WiseQueue.Core.Server;
using WiseQueue.Domain.Configuration;
using WiseQueue.Domain.MsSql;
using WiseQueue.Domain.NLogger;

namespace PerformanceTests.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=DESKTOP-M3DHIK5;" +
                               "Initial Catalog=WiseQueues;" +
                               "Integrated Security=SSPI;";
           
            ServerConfiguration serverConfiguration = new ServerConfiguration(20);

            using (IWiseQueueConfiguration configuration = WiseQueueGlobalConfiguration.WiseQueueConfiguration
                .UseNLog()
                .UseServer(serverConfiguration)
                .UseSqlServer(connectionString))
            {
                configuration.Activate();

                Console.WriteLine("**** Waiting for tasks...");
                Console.ReadLine();
            }
        }
    }
}

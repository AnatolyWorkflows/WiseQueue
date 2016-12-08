using System;
using WiseQueue.Core.Common;
using WiseQueue.Core.Server;
using WiseQueue.Domain.Common;
using WiseQueue.Domain.MsSql;
using WiseQueue.Domain.NLogger;
using WiseQueue.Domain.Server;

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
                Console.WriteLine("**** Waiting for tasks...");
                Console.ReadLine();
            }
        }
    }
}

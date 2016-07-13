using System;
using WiseQueue.Core.Common;
using WiseQueue.Domain.Client;
using WiseQueue.Domain.Client.Management;
using WiseQueue.Domain.Common;
using WiseQueue.Domain.NLogger;
using WiseQueue.Domain.MsSql;
using WiseQueue.Domain.Server;

namespace WiseQueue
{
    class Program
    {
        private class MyClass
        {
            public void Test(string msg)
            {
                Console.WriteLine("MSG: {0}", msg);
            }
        }


        static void Main(string[] args)
        {
            string connectionString = "connection string";

            connectionString = "Data Source=(local);" +
                               "Initial Catalog=WiseQueues;" +
                               "Integrated Security=SSPI;";


            using (IWiseQueueConfiguration configuration = WiseQueueGlobalConfiguration.WiseQueueConfiguration
                .UseNLog()
                .UseClient()
                .UseServer()
                .UseSqlServer(connectionString))
            {
                configuration.Activate();

                Int64 taskId = ClientManager.StartNewTask(() => new MyClass().Test("Hello"));
                
                Console.ReadLine();
            }
        }
    }
}

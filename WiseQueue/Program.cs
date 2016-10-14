using System;
using System.Threading;
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

            public void VerySlowTask(string msg, CancellationToken cancellationToken)
            {
                while (true)
                {
                    Console.WriteLine("MSG: {0}", msg);
                    Thread.Sleep(1000);

                    if (cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine("Cancelling...");
                        break;
                    }
                }             
                Console.WriteLine("The VerySlowTask has been cancelled.");   
            }
        }


        static void Main(string[] args)
        {
            string connectionString = "connection string";

            connectionString = "Data Source=DESKTOP-M3DHIK5;" +
                               "Initial Catalog=WiseQueues;" +
                               "Integrated Security=SSPI;";


            using (IWiseQueueConfiguration configuration = WiseQueueGlobalConfiguration.WiseQueueConfiguration
                .UseNLog()
                .UseClient()
                .UseServer()
                .UseSqlServer(connectionString))
            {
                configuration.Activate();

                //Int64 taskId = ClientManager.StartNewTask(() => new MyClass().Test("Hello"));

                Int64 taskId = ClientManager.StartNewTask(() => new MyClass().VerySlowTask("Very slow task...", new CancellationToken()));

                Console.WriteLine("PRESS ENTER TO CANCEL");
                Console.ReadLine();

                ClientManager.CancelTask(taskId);

                Console.ReadLine();
            }
        }
    }
}

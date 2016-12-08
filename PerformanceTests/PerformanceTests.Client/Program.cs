using System;
using PerformanceTest.Common;
using WiseQueue.Core.Common;
using WiseQueue.Domain.NLogger;
using WiseQueue.Domain.Client;
using WiseQueue.Domain.MsSql;
using ClientConfiguration = WiseQueue.Core.Client.ClientConfiguration;
using WiseQueueGlobalConfiguration = WiseQueue.Domain.Common.WiseQueueGlobalConfiguration;

namespace PerformanceTests.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=DESKTOP-M3DHIK5;" +
                               "Initial Catalog=WiseQueues;" +
                               "Integrated Security=SSPI;";

            string performanceConnectionString = "Data Source=DESKTOP-M3DHIK5;" +
                               "Initial Catalog=PerformanceTests;" +
                               "Integrated Security=SSPI;";

            int maxCount = 1000;
            if (args.Length > 0)
                maxCount = Convert.ToInt32(args[0]);

            Console.WriteLine("Maximum requests = {0}", maxCount);

            ClientConfiguration clientConfiguration = new ClientConfiguration(10, TimeSpan.FromSeconds(30));

            using (IWiseQueueConfiguration configuration = WiseQueueGlobalConfiguration.WiseQueueConfiguration
                .UseNLog()                
                .UseSqlServer(connectionString)
                .UseClient(clientConfiguration))
            {
                for (int i = 0; i < maxCount; i++)
                {
                    Guid requestId = Guid.NewGuid();
                    Console.WriteLine("Enqueueing requestId = {0}...", requestId);
                    Int64 taskId = BackgroundClient.StartNewTask(() => new WithExceptionServerJob().Execute(requestId, performanceConnectionString));
                    Console.WriteLine("The request has been enqueued. TaskId: {0}", taskId);
                }                

                Console.WriteLine("All requests have been enqueued.");
                Console.ReadLine();
            }
        }
    }
}

using System;
using WiseQueue.Core.Common;
using WiseQueue.Domain.Common;
using WiseQueue.Domain.NLogger;
using WiseQueue.Domain.MsSql;

namespace WiseQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "connection string";
            using (IWiseQueueConfiguration configuration = WiseQueueGlobalConfiguration.WiseQueueConfiguration
                .UseNLog()
                .UseSqlServer(connectionString))
            {
                Console.ReadLine();
            }
        }
    }
}

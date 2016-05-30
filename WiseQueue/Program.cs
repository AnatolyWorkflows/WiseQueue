using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;
using WiseQueue.Core.Common.Logging;
using WiseQueue.Domain.NLogger;

namespace WiseQueue
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var kernel = new StandardKernel())
            {
                //TODO: Remove to Global Configuration.
                kernel.Load(new NLogLoggerNinjectModule());

                IWiseQueueLoggerFactory loggerFactory = kernel.Get<IWiseQueueLoggerFactory>();
                IWiseQueueLogger logger = loggerFactory.Create("Main");
                logger.WriteInfo("Press enter...");

                Console.ReadLine();
            }
        }
    }
}

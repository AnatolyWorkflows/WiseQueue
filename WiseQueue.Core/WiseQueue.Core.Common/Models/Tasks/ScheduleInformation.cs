using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WiseQueue.Core.Common.Models.Tasks
{
    public class ScheduleInformation
    {
        public int RepeatCrashCount { get; private set; }

        public ScheduleInformation(int repeatCrashCount)
        {
            RepeatCrashCount = repeatCrashCount;
        }
    }
}

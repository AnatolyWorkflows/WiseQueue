using System;
using WiseQueue.Core.Common.Management;

namespace WiseQueue.Core.Common
{
    public class TaskManagerConfiguration
    {
        public int MaxTaskPerQueue { get; set; }
        public TimeSpan TimeShiftAfterCrash { get; set; }
        public int MaxRerunAttempts { get; set; }

        public TaskManagerState State { get; set; }

        public TaskManagerConfiguration()
        {
            MaxTaskPerQueue = 4;
            MaxRerunAttempts = 2;
            TimeShiftAfterCrash = TimeSpan.FromSeconds(20);
            State = TaskManagerState.Both;
        }
    }
}

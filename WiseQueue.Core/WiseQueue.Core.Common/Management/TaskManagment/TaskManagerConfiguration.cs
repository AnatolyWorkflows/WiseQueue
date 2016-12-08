using System;

namespace WiseQueue.Core.Common.Management.TaskManagment
{
    public class TaskManagerConfiguration
    {
        public int MaxTaskPerQueue { get; set; }
        public TimeSpan TimeShiftAfterCrash { get; set; }
        public int MaxRerunAttempts { get; set; }

        public TaskManagerConfiguration()
        {
            MaxTaskPerQueue = 4;
            MaxRerunAttempts = 2;
            TimeShiftAfterCrash = TimeSpan.FromSeconds(20);
        }
    }
}

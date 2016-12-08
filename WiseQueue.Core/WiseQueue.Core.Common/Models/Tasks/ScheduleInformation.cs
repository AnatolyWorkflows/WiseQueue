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

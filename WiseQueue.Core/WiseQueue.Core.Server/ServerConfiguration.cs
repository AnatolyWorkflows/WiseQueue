using System;

namespace WiseQueue.Core.Server
{
    public class ServerConfiguration
    {
        public int MaxTaskPerQueue { get; set; }
        public TimeSpan TimeShiftAfterCrash { get; set; }
        public int MaxRerunAttempts { get; set; }

        public ServerConfiguration()
        {
            MaxTaskPerQueue = 4;
            MaxRerunAttempts = 2;
            TimeShiftAfterCrash = TimeSpan.FromSeconds(20);
        }
    }
}

using System;

namespace WiseQueue.Core.Client
{
    public class ClientConfiguration
    {
        public static ClientConfiguration Default
        {
            get { return new ClientConfiguration(10, TimeSpan.FromSeconds(30)); }
        }

        public int MaxRerunAttempts { get; private set; }

        public TimeSpan TimeShiftAfterCrash { get; private set; }

        public ClientConfiguration(int maxRerunAttempts, TimeSpan timeShiftAfterCrash)
        {
            MaxRerunAttempts = maxRerunAttempts;
            TimeShiftAfterCrash = timeShiftAfterCrash;
        }
    }
}

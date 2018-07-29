using System;

namespace BackupFramework
{
    public class Policy
    {
        public int BackupCount { get; }
        public int Interval { get; }
        public Policy(int interval, int count)
        {
            BackupCount = count;
            Interval = interval;
        }
    }
}
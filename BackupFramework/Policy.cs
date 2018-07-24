namespace BackupFramework
{
    public class Policy
    {
        private int count;
        private int interval;
        public int Count { get { return count; } }
        public int Interval { get { return interval; } }
        public Policy(int count, int interval)
        {
            this.count = count;
            this.interval = interval;
        }
    }
}

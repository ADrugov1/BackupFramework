namespace BackupFramework
{
    using System.Collections;
    public class PolicyComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            var a = x as Policy;
            var b = y as Policy;
            if (a.Interval == b.Interval)
            {
                return a.BackupCount > b.BackupCount ? 1 : a.BackupCount == b.BackupCount ? 0 : -1;
            }
            return a.Interval > b.Interval ? 1 : -1;
        }
    }
}

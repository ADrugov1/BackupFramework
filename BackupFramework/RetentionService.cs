namespace BackupFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class RetentionService
    {
        private const string CONST_BackupPath = @"C:\Backup\";
        public List<Policy> Policies { get; private set; } = new List<Policy>();
        private string backupPath;
        private int checkInterval;

        public RetentionService(int interval = 1, string path = CONST_BackupPath)
        {
            checkInterval = interval > 0 ? interval : 1;
            backupPath = string.IsNullOrEmpty(path) ? CONST_BackupPath : path;
        }
        public void Action()
        {
            Console.WriteLine("RetentionService action START");
            var previousCleaningTime = DateTime.Now;
            while (true)
            {
                var time = DateTime.Now;
                var interval = (time - previousCleaningTime).Days;

                if (interval > checkInterval)
                {
                    previousCleaningTime = time;
                    DeleteOldBackups(backupPath);
                }
            }
        }

        internal void DeleteOldBackups(string path = CONST_BackupPath)
        {
            var dir = new DirectoryInfo(path);
            var files = dir.GetFiles().ToList();
            var now = DateTime.UtcNow;

            foreach (var policy in Policies)
            { 
                var olderBackups = files.Where(f => (now - f.CreationTimeUtc).Days > policy.Interval).OrderByDescending(f => f.CreationTimeUtc).ToList();
                for (var i = policy.BackupCount; i < olderBackups.Count; i++)
                {
                    files.Remove(olderBackups[i]);
                    File.Delete(olderBackups[i].FullName);
                    Console.WriteLine("Delete backup older {0} days {1}", policy.Interval, olderBackups[i].FullName);
                }
            }
        }

        public void CheckPolicies()
        {
            Policies = Policies.OrderByDescending(x => x.Interval).ToList();

            for (var i = 1; i < Policies.Count; i++)
            {
                if (Policies[i].Interval == Policies[i - 1].Interval)
                {
                    if (Policies[i].BackupCount < Policies[i - 1].BackupCount)
                    {
                        Policies.Remove(Policies[i]);
                    }
                    else
                    {
                        Policies.Remove(Policies[i - 1]);
                    }
                    i--;
                    continue;
                }
                if (Policies[i].BackupCount < Policies[i - 1].BackupCount)
                {
                    Policies.Remove(Policies[i]);
                    i--;
                }
            }
        }

        public void Add(Policy policy)
        {
            Policies.Add(policy);
            CheckPolicies();
        }
        public void Add(List<Policy> policies)
        {
            Policies.AddRange(policies);
            CheckPolicies();
        }
    }
}

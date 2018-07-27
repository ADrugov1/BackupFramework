namespace BackupFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class RetentionService
    {
        private const string CONST_BackupPath = @"C:\Backup\";
        private List<Policy> Policies;
        private string backupPath;
        private int checkInterval;

        public RetentionService(string path, int interval, List<Policy> policies)
        {
            Policies = CheckPolicies(policies); 
            checkInterval = interval > 0 ? interval : 1;
            backupPath = String.IsNullOrEmpty(path) ? CONST_BackupPath : path;
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

                    DirectoryInfo dir = new DirectoryInfo(CONST_BackupPath);
                    var files = dir.GetFiles().ToList();

                    DeleteOldBackups(files, Policies);
                }
            }
        }

        private void DeleteOldBackups(List<FileInfo> files, IEnumerable<Policy> policies)
        {
            var now = DateTime.UtcNow;

            foreach(var policy in policies)
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

        public List<Policy> CheckPolicies(List<Policy> policies)
        {
            if (policies == null || policies.Count == 0) return new List<Policy>();

            policies = policies.OrderByDescending(x => x.Interval).ToList();

            for (var i = 1;  i < policies.Count; i++)
            {
                if (policies[i].BackupCount < policies[i - 1].BackupCount)
                {
                    policies.Remove(policies[i]);
                    i--;
                }
            }
            return policies;
        }
    }
}

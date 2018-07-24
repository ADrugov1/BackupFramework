namespace BackupFramework
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public class RetentionService : ServiceBase
    {
        private List<Policy> Policies = new List<Policy>();

        public RetentionService(): base()
        {
            Policies = new List<Policy> {
                new Policy(int.Parse(appSettings["ThreeDaysCount"]), int.Parse(appSettings["ThreeDaysInterval"])),
                //new Policy(int.Parse(appSettings["OneWeekCount"]), int.Parse(appSettings["OneWeekInterval"])),
                //new Policy(int.Parse(appSettings["TwoWeekCount"]), int.Parse(appSettings["TwoWeekInterval"]))
            };
        }
        public void Action()
        {
            Console.WriteLine("RetentionService action START");
            var previousCleaningTime = DateTime.Now;
            while (true)
            {
                var time = DateTime.Now;
                var interval = (time - previousCleaningTime).Seconds;

                if (interval > timesMultiplier * backupInterval / backupCount)
                {
                    previousCleaningTime = time;

                    DirectoryInfo dir = new DirectoryInfo(CONST_BackupPath);
                    var files = dir.GetFiles().ToList();

                    DeleteOldBackups(files, Policies);
                }
            }
        }

        private void DeleteOldBackups(List<FileInfo> files, List<Policy> policies)
        {
            var now = DateTime.UtcNow;

            foreach(var policy in policies)
            { 
                var olderBackups = files.Where(f => (now - f.CreationTimeUtc).Seconds > policy.Interval * timesMultiplier).OrderByDescending(f => f.CreationTimeUtc).ToArray();
                for (var i = policy.Count; i < olderBackups.Length; i++)
                {
                    files.Remove(olderBackups[i]);
                    File.Delete(olderBackups[i].FullName);
                    Console.WriteLine("Delete backup older {0} days {1}", policy.Interval, olderBackups[i].FullName);
                }
            }
        }
    }
}

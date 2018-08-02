namespace BackupFramework
{
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    class Program
    {
        static void Main(string[] args)
        {
            var backupPath = @"C:\Backup\";
            new DirectoryInfo(backupPath).Create();

             var policies = new List<Policy>() //// Keep no more than COUNT backups older than INTERVAL days;
             {
                 new Policy(14, 1),
                 new Policy(7, 4),
                 new Policy(3, 4)
             };

            var backupService = new BackupService(backupPath, 5, 2);
            var retentionService = new RetentionService(1, backupPath);
            retentionService.Add(policies);
            Parallel.Invoke(backupService.Action, retentionService.Action);
        }
    }
}

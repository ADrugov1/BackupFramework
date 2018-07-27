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
                 new Policy(4, 1),//// conflict. Will be deleted
                 new Policy(3, 12),
                 new Policy(2, 9),
                 new Policy(1, 10),
                 new Policy(7, 1)//// conflict. Will be deleted
             };

            var backupService = new BackupService(backupPath, 5, 2);
            var retentionService = new RetentionService(backupPath, 1,policies);

            Parallel.Invoke(backupService.Action, retentionService.Action);
        }
    }
}

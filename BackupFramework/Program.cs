using System.IO;
using System.Threading;

namespace BackupFramework
{
    class Program
    {
        static void Main(string[] args)
        {
            new DirectoryInfo(ServiceBase.CONST_BackupPath).Create();
            var backupService = new BackupService();
            var thread = new Thread(new ThreadStart(backupService.Action));
            thread.Start();

            var retentionService = new RetentionService();
            retentionService.Action();
        }
    }
}

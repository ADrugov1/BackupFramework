namespace BackupFramework
{
    using System;
    using System.IO;
    using System.Text;
    public class BackupService : ServiceBase
    {
        public BackupService() : base() { }

        public void CreateBackup()
        {
            var fileName = new Random().Next().ToString();
            var builder = new StringBuilder(CONST_BackupPath).Append(fileName).Append(".txt");

            var file = File.Create(builder.ToString()); ////Empty file aka backup
            file.Close();

            Console.WriteLine("Create backup with name {0}", builder.ToString());
        }
        public void Action()
        {
            Console.WriteLine("BackupService action START");

            var previousBackupTime = DateTime.Now;
            while (true)
            {
                var time = DateTime.Now;
                var interval = (time - previousBackupTime).Seconds;
                if ( interval > timesMultiplier * backupInterval / backupCount)
                {
                    previousBackupTime = time;
                    CreateBackup();
                }
            }
        }
    }
}

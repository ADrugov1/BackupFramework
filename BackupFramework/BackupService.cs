namespace BackupFramework
{
    using System;
    using System.IO;
    using System.Text;
    public class BackupService
    {
        private const string CONST_BackupPath = @"C:\Backup\";

        private string backupPath;
        private int backupInterval;
        private int backupCount;

        public BackupService(string path, int interval, int count)
        {
            backupCount = count > 0 ? count : 1;
            backupInterval = interval > 0 ? interval : 2;
            backupPath = String.IsNullOrEmpty(path) ? CONST_BackupPath : path;
        }

        public void CreateBackup()
        {
            var fileName = new Random().Next().ToString();
            var builder = new StringBuilder(backupPath).Append(fileName).Append(".txt");

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
                var interval = (time - previousBackupTime).Days;
                if ( interval > backupInterval / backupCount)
                {
                    previousBackupTime = time;
                    CreateBackup();
                }
            }
        }
    }
}

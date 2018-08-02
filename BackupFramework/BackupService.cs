namespace BackupFramework
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;

    public class BackupService
    {
        private const string CONST_BackupPath = @"C:\Backup\";
        private const int CONST_MillisecondsInDay = 86400000;
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

            var interval = CONST_MillisecondsInDay * backupInterval / backupCount;
            while (true)
            {
                Thread.Sleep(interval);
                CreateBackup();
            }
        }
    }
}

namespace BackupFramework
{
    using System;
    using System.Collections.Specialized;
    abstract public class ServiceBase
    {
        public static string CONST_BackupPath = @"C:/Backup/";
        protected int backupCount;
        protected int backupInterval;
        protected int timesMultiplier;
        protected NameValueCollection appSettings;
        public ServiceBase()
        {
            appSettings = System.Configuration.ConfigurationManager.AppSettings;
            backupCount = int.Parse(appSettings["BackupCount"]);
            backupInterval = int.Parse(appSettings["BackupInterval"]);
            timesMultiplier = (int)Enum.Parse(typeof(Times), appSettings["times"]);
        }
    }
}

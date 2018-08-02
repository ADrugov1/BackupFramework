namespace BackupFramework.Tests
{
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    [TestFixture]
    public class RetentionServiceTests
    {
        [Test]
        public void CheckPoliciesShouldReturnUniquePoliciesList()
        {
            //// Arrange
            var policies = new List<Policy>()
             {
                 new Policy(3, 2),
                 new Policy(3, 10),
                 new Policy(3, 12),
                 new Policy(4, 1),
                 new Policy(2, 9),
                 new Policy(1, 10),
                 new Policy(7, 1)
             };
            var expected = new List<Policy>()
            {
                 new Policy(7, 1),
                 new Policy(4, 1),
                 new Policy(3, 12)
            };

            var sut = new RetentionService();

            sut.Add(policies);
            var comparer = new PolicyComparer();

            //// Act
            sut.CheckPolicies();

            ////Assert
            CollectionAssert.AreEqual(sut.Policies, expected, comparer);         
        }

        [Test]
        [TestCase(7,4)]
        public void DeleteOldBackups_ShouldNotDeleteFiles_IfSatisfyPolicy(int interval, int count)
        {
            //// Arrange
            var policies = new List<Policy>() {new Policy(interval, count)};
            var sut = new RetentionService();
            var path = @"C:/BackupTest/";
            var dir = new DirectoryInfo(path);

            dir.Create();
            sut.Add(policies);

            for(var i=0; i< count;i++) CreateTestFile(path + "File"+ i +".txt", interval + 1);

            ////Act
            sut.DeleteOldBackups(path);

            ////Assert
            Assert.AreEqual(count, dir.GetFiles().Count());
            
            Directory.Delete(path, true);
        }

        [Test]
        [TestCase(7, 4)]
        public void DeleteOldBackups_ShouldDeleteFiles_IfNotSatisfyPolicy(int interval, int count)
        {
            //// Arrange
            var policies = new List<Policy>() { new Policy(interval, count)};
            var sut = new RetentionService();
            var path = @"C:/BackupTest/";
            var dir = new DirectoryInfo(path);

            dir.Create();
            sut.Add(policies);

            for (var i = 0; i < count + 5 ; i++) CreateTestFile(path + "File" + i + ".txt", interval + 1);

            ////Act
            sut.DeleteOldBackups(path);

            ////Assert
            Assert.AreEqual(count, dir.GetFiles().Count());

            Directory.Delete(path, true);
        }

        [Test]
        public void DeleteOldBackups_ShouldNotDeleteFiles_IfPolicyIsEmpty()
        {
            //// Arrange
            var policies = new List<Policy>();

            var sut = new RetentionService();
            var path = @"C:/BackupTest/";
            var dir = new DirectoryInfo(path);

            dir.Create();
            sut.Add(policies);

            var count = 5;
            for (var i = 0; i < count; i++) CreateTestFile(path + "File" + i + ".txt", interval + 1);

            ////Act
            sut.DeleteOldBackups(path);

            ////Assert
            Assert.AreEqual(count, dir.GetFiles().Count());

            Directory.Delete(path, true);
        }

        [Test]
        public void DeleteOldBackups_ShouldDeleteFiles_IfNotSatisfyMultiplyPlolicy()
        {
            //// Arrange
            var policies = new List<Policy>() {
                 new Policy(14, 1),
                 new Policy(7, 4),
                 new Policy(3, 4)};

            var sut = new RetentionService();
            var path = @"C:/BackupTest/";
            var dir = new DirectoryInfo(path);

            dir.Create();
            sut.Add(policies);

            for (var i = 0; i < 3; i++) CreateTestFile(path + "File" + i + ".txt", 15);
            for (var i = 3; i < 6; i++) CreateTestFile(path + "File" + i + ".txt", 8);
            for (var i = 6; i < 9; i++) CreateTestFile(path + "File" + i + ".txt", 5);
            for (var i = 9; i < 12; i++) CreateTestFile(path + "File" + i + ".txt", 2);

            ////Act
            sut.DeleteOldBackups(path);

            ////Assert
            Assert.AreEqual(7, dir.GetFiles().Count());

            Directory.Delete(path, true);
        }

        private void CreateTestFile(string fileName, int daysBefore)
        {
            File.Create(fileName).Close();
            File.SetCreationTime(fileName, DateTime.Now.AddDays(-daysBefore));
        }
    }
}
